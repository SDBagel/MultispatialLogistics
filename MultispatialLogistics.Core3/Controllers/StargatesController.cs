using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultispatialLogistics.Core3.Models;

namespace MultispatialLogistics.Core3.Controllers
{
    public class StargatesController : Controller
    {
        private readonly MultispatialLogisticsContext _context;
        public StargatesController(MultispatialLogisticsContext context)
        {
            _context = context;
        }

        // GET: Stargates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stargate.ToListAsync());
        }

        // GET: Stargates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var stargate = await _context.Stargate
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stargate == null)
                return NotFound();

            return View(stargate);
        }
    }
}
