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
    public class GardenWorkDaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenWorkDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenWorkDays
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenWorkDay.Include(g => g.BaseSubType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenWorkDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var GardenWorkDay = await _context.GardenWorkDay
                .Include(g => g.BaseSubType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (GardenWorkDay == null)
            {
                return NotFound();
            }

            return View(GardenWorkDay);
        }

        // GET: GardenWorkDays/Create
        public IActionResult Create()
        {
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id");
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            return View();
        }

        // POST: GardenWorkDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkingTime,GardenTaskId,IsMon,IsTue,IsWed,IsThu,IsFri,IsSat,IsSun,SubTypeId,CreateDate")] GardenWorkDay GardenWorkDay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(GardenWorkDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", GardenWorkDay.SubTypeId);
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            return View(GardenWorkDay);
        }

        // GET: GardenWorkDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var GardenWorkDay = await _context.GardenWorkDay.FindAsync(id);
            if (GardenWorkDay == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", GardenWorkDay.SubTypeId);
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            return View(GardenWorkDay);
        }

        // POST: GardenWorkDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkingTime,GardenTaskId,IsMon,IsTue,IsWed,IsThu,IsFri,IsSat,IsSun,SubTypeId,CreateDate")] GardenWorkDay GardenWorkDay)
        {
            if (id != GardenWorkDay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(GardenWorkDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenWorkDayExists(GardenWorkDay.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", GardenWorkDay.SubTypeId);
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            return View(GardenWorkDay);
        }

        // GET: GardenWorkDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var GardenWorkDay = await _context.GardenWorkDay
                .Include(g => g.BaseSubType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (GardenWorkDay == null)
            {
                return NotFound();
            }

            return View(GardenWorkDay);
        }

        // POST: GardenWorkDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var GardenWorkDay = await _context.GardenWorkDay.FindAsync(id);
            _context.GardenWorkDay.Remove(GardenWorkDay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenWorkDayExists(int id)
        {
            return _context.GardenWorkDay.Any(e => e.Id == id);
        }
    }
}
