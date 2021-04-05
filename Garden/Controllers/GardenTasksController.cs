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
    public class GardenTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenTask.Include(g => g.BaseSubType).Include(g => g.GardenSpace);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTask = await _context.GardenTask
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenTask == null)
            {
                return NotFound();
            }

            return View(gardenTask);
        }

        // GET: GardenTasks/Create
        public IActionResult Create()
        {
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id");
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id");
            return View();
        }

        // POST: GardenTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SubTypeId,IsActivate,CreateDate,GardenSpaceId")] GardenTask gardenTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenTask.SubTypeId);
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenTask.GardenSpaceId);
            return View(gardenTask);
        }

        // GET: GardenTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTask = await _context.GardenTask.FindAsync(id);
            if (gardenTask == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenTask.SubTypeId);
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenTask.GardenSpaceId);
            return View(gardenTask);
        }

        // POST: GardenTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SubTypeId,IsActivate,CreateDate,GardenSpaceId")] GardenTask gardenTask)
        {
            if (id != gardenTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenTaskExists(gardenTask.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenTask.SubTypeId);
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenTask.GardenSpaceId);
            return View(gardenTask);
        }

        // GET: GardenTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTask = await _context.GardenTask
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenTask == null)
            {
                return NotFound();
            }

            return View(gardenTask);
        }

        // POST: GardenTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenTask = await _context.GardenTask.FindAsync(id);
            _context.GardenTask.Remove(gardenTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenTaskExists(int id)
        {
            return _context.GardenTask.Any(e => e.Id == id);
        }
    }
}
