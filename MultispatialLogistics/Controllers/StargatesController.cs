using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultispatialLogistics.Models;

namespace MultispatialLogistics.Controllers
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
            {
                return NotFound();
            }

            var stargate = await _context.Stargate
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stargate == null)
            {
                return NotFound();
            }

            return View(stargate);
        }

        // GET: Stargates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stargates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ParentSystemId,ParentSystemName,StargateId,DestinationSystemId,DestinationStargateId,XPos,YPos,ZPos")] Stargate stargate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stargate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stargate);
        }

        // GET: Stargates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stargate = await _context.Stargate.FindAsync(id);
            if (stargate == null)
            {
                return NotFound();
            }
            return View(stargate);
        }

        // POST: Stargates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParentSystemId,ParentSystemName,StargateId,DestinationSystemId,DestinationStargateId,XPos,YPos,ZPos")] Stargate stargate)
        {
            if (id != stargate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stargate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StargateExists(stargate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(stargate);
        }

        // GET: Stargates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stargate = await _context.Stargate
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stargate == null)
            {
                return NotFound();
            }

            return View(stargate);
        }

        // POST: Stargates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stargate = await _context.Stargate.FindAsync(id);
            _context.Stargate.Remove(stargate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StargateExists(int id)
        {
            return _context.Stargate.Any(e => e.Id == id);
        }
    }
}
