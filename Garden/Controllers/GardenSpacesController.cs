using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garden.Data;
using Garden.Models;

namespace Garden.Controllers
{
    public class GardenSpacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenSpacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenSpaces
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenSpace.Include(g => g.BaseSubType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenSpaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSpace = await _context.GardenSpace
                .Include(g => g.BaseSubType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenSpace == null)
            {
                return NotFound();
            }

            return View(gardenSpace);
        }

        // GET: GardenSpaces/Create
        public IActionResult Create()
        {
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id");
            return View();
        }

        // POST: GardenSpaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SubTypeId,CreatedDate,IsActivate")] GardenSpace gardenSpace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenSpace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenSpace.SubTypeId);
            return View(gardenSpace);
        }

        // GET: GardenSpaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSpace = await _context.GardenSpace.FindAsync(id);
            if (gardenSpace == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenSpace.SubTypeId);
            return View(gardenSpace);
        }

        // POST: GardenSpaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SubTypeId,CreatedDate,IsActivate")] GardenSpace gardenSpace)
        {
            if (id != gardenSpace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenSpace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenSpaceExists(gardenSpace.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenSpace.SubTypeId);
            return View(gardenSpace);
        }

        // GET: GardenSpaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSpace = await _context.GardenSpace
                .Include(g => g.BaseSubType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenSpace == null)
            {
                return NotFound();
            }

            return View(gardenSpace);
        }

        // POST: GardenSpaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenSpace = await _context.GardenSpace.FindAsync(id);
            _context.GardenSpace.Remove(gardenSpace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenSpaceExists(int id)
        {
            return _context.GardenSpace.Any(e => e.Id == id);
        }
    }
}
