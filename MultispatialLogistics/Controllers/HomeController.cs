﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultispatialLogistics.Models;
using Newtonsoft.Json;

namespace MultispatialLogistics.Controllers
{
    public class HomeController : Controller
    {
        private string token;
        private string CharacterID;

        private readonly MultispatialLogisticsContext _context;
        public HomeController(MultispatialLogisticsContext context)
        {
            _context = context;
        }

        private string AccessToken(string code)
        {
            string url = @"https://login.eveonline.com/oauth/token";

            //POST to CCP for token and return string token from the acquired json file
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Authorization] = "Basic ODIwZGMyNzQ3ZjQyNDgyZjgzZjJhNmU5MGFiNWE4ZTI6b0tUbmZZcDZBMzF5Ym9ka1lBQzluYnJ2cnNubEw1cXFRMmVMVHlWcw==";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers[HttpRequestHeader.Host] = "login.eveonline.com";
                client.Headers["grant_type"] = "authorization_code";

                string data = string.Format(@"grant_type=authorization_code&code={0}&redirect_uri=http://localhost:44349/", code);
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(client.UploadString(url, data))["access_token"];
            }
        }

        private string GetCharacterID(string token)
        {
            string url = @"https://login.eveonline.com/oauth/verify/";

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Authorization] = $"Bearer {token}";
                client.Headers[HttpRequestHeader.Host] = "login.eveonline.com";

                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(client.DownloadString(url))["CharacterID"].ToString();
            }
        }

        private Dictionary<string, dynamic> GetTQResource(string path, string token = null, string flags = null)
        {
            string host = "https://esi.evetech.net/latest";
            string url = $"{host}{path}?datasource=tranquility";
            if (token != null)
                url += $"&token={token}";
            if (flags != null)
                url += $"&{flags}";

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Authorization] = $"Bearer {token}";
                client.Headers[HttpRequestHeader.Host] = "esi.evetech.net";

                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(client.DownloadString(url));
            }
        }

        private List<long> GetRoute(int originID, int destinationID, string flag)
        {
            string host = "https://esi.evetech.net/latest";
            string url = $"{host}/route/{originID}/{destinationID}/?datasource=tranquility&flag={flag}";

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.Authorization] = $"Bearer {token}";
                client.Headers[HttpRequestHeader.Host] = "esi.evetech.net";

                return JsonConvert.DeserializeObject<List<long>>(client.DownloadString(url));
            }
        }

        private double GetRouteTime(List<long> systems, string shipName, int perSystemOffset)
        {
            double totalTime = 0;

            //TODO: get these from local SQL database from shipTypeID
            double maxWarpSpeed = 11.27;
            double maxSubwarpSpeed = 328;
            
            foreach (long systemID in systems)
            {
                //get all stargates within this system
                IEnumerable<Stargate> stargates = from gate in _context.Stargate.ToList()
                                                  where gate.ParentSystemId == systemID
                                                  select gate;

                double totalDist = 0;
                //doesnt do time calculation if first system (assuming starting at first gate)
                if (systemID != systems.First() && systemID != systems.Last())
                {
                    //find next and previous gates
                    Stargate nextGate = (from gate in stargates
                                         where gate.DestinationSystemId == systems[systems.IndexOf(systemID) + 1]
                                         select gate).FirstOrDefault();
                    Stargate prevGate = (from gate in stargates
                                         where gate.DestinationSystemId == systems[systems.IndexOf(systemID) - 1]
                                         select gate).FirstOrDefault();

                    //using two pythagorean theorems with X, Y, and Z dist, find direct distance
                    double xyDist = Math.Sqrt(Math.Pow(nextGate.XPos - prevGate.XPos, 2) + Math.Pow(nextGate.YPos - prevGate.YPos, 2));
                    totalDist = Math.Sqrt(Math.Pow(xyDist, 2) + Math.Pow(nextGate.ZPos - prevGate.ZPos, 2));

                    double k_decel = Math.Min(maxWarpSpeed / 3, 2);

                    double warp_dropout_speed = Math.Min(maxSubwarpSpeed / 2, 100);
                    double max_ms_warp_speed = maxWarpSpeed * 149597870700;

                    double decel_dist = max_ms_warp_speed / k_decel;

                    double minimum_dist = 149597870700 + decel_dist;

                    double cruise_time = 0;
                    if (minimum_dist > totalDist)
                        max_ms_warp_speed = totalDist * maxWarpSpeed * k_decel / (maxWarpSpeed + k_decel);
                    else
                        cruise_time = (totalDist - minimum_dist) / max_ms_warp_speed;

                    //accel, decel, cruise times respectively
                    totalTime += Math.Log(max_ms_warp_speed / maxWarpSpeed) / maxWarpSpeed;
                    totalTime += Math.Log(max_ms_warp_speed / warp_dropout_speed) / k_decel;
                    totalTime += cruise_time;
                    //add 20 seconds to account for system change times
                    totalTime += 20;
                    //TODO: add ship align times 

                    /* from db get ship using shipName
                     * (ln(2) * ship.Inertia +- player inertia modifiers * ship.Mass +- fit modifiers) / 500,000
                     * Example values: 2.1688 & 1075000 (1.075 tonnes) for slasher
                    */

                    //add buffer for not perfect to-warp transition times
                    totalTime += perSystemOffset;
                }
            }
            return totalTime;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            //If there is no token from Callback(), redirect to login
            token = Request.Cookies["accessToken"];
            if (token == null)
            {
                Response.Redirect("../login");
            }
            else
            {
                //Try to extract route info from url
                try
                {
                    //Converts o (origin) and d (destination) flags into usable IDs from URL
                    int origin = (from gate in _context.Stargate.ToList()
                                  where gate.ParentSystemName == Request.Query["o"]
                                  select gate).FirstOrDefault().ParentSystemId;
                    int destination = (from gate in _context.Stargate.ToList()
                                       where gate.ParentSystemName == Request.Query["d"]
                                       select gate).FirstOrDefault().ParentSystemId;
                    string ship = Request.Query["s"];

                    if (ship == null)
                        throw new ArgumentException();

                    //Gets route and outputs to view
                    //TODO: scoop this into a method that generates partial view that looks nice
                    List<long> route = GetRoute(origin, destination, "secure");
                    string a = "";
                    foreach (long l in route)
                    {
                        a += (from gate in _context.Stargate.ToList()
                              where gate.ParentSystemId == l
                              select gate).FirstOrDefault().ParentSystemName + " ";
                    }
                    ViewData["Route"] = a;
                    //Display estimated range of values for route time
                    ViewData["Message"] = Math.Round((GetRouteTime(route, ship, 5) / 60), 2).ToString() + 
                                  " - " + Math.Round((GetRouteTime(route, ship, 10) / 60), 2).ToString();
                }
                catch
                {
                    if (Request.Query.Count != 0)
                        ViewData["Message"] = "Invalid route URL: usage - \"?o=Jita&d=Perimeter&s=Velator\" " +
                                              "Check system and ship names are valid, and all entries are satisfied.";
                }
                
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(string origin, string destination)
        {
            try
            {
                int o = (from gate in _context.Stargate.ToList()
                         where gate.ParentSystemName == origin
                         select gate).FirstOrDefault().ParentSystemId;
                int d = (from gate in _context.Stargate.ToList()
                         where gate.ParentSystemName == destination
                         select gate).FirstOrDefault().ParentSystemId;
                string ship = "";

                //Gets route and outputs to view
                //TODO: scoop this into a method that generates partial view that looks nice
                List<long> route = GetRoute(o, d, "secure");
                string a = "";
                foreach (long l in route)
                {
                    a += (from gate in _context.Stargate.ToList()
                          where gate.ParentSystemId == l
                          select gate).FirstOrDefault().ParentSystemName + " ";
                }
                ViewData["Route"] = a;
                //Display estimated range of values for route time
                ViewData["Message"] = Math.Round((GetRouteTime(route, ship, 5) / 60), 2).ToString() +
                              " - " + Math.Round((GetRouteTime(route, ship, 10) / 60), 2).ToString();
            }
            catch
            { }
            return View();
        }

        public IActionResult Callback()
        {
            //Using AccessToken() function with code parameter, add token as a cookie
            Response.Cookies.Append(
                "accessToken",
                AccessToken(Request.Query["code"]),
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(20),
                    Path = "/"
                }
            );
            Response.Redirect("../");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
