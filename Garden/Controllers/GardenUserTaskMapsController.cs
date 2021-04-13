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
    public class GardenUserTaskMapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenUserTaskMapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenUserTaskMaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenUserTaskMap.Include(g => g.GardenTask);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenUserTaskMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUserTaskMap = await _context.GardenUserTaskMap
                .Include(g => g.GardenTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenUserTaskMap == null)
            {
                return NotFound();
            }

            return View(gardenUserTaskMap);
        }

        // GET: GardenUserTaskMaps/Create
        public IActionResult Create()
        {
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id");
            return View();
        }

        // POST: GardenUserTaskMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GardenUserId,GardenTaskId")] GardenUserTaskMap gardenUserTaskMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenUserTaskMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenUserTaskMap.GardenTaskId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenUserTaskMap.GardenUserId);
            return View(gardenUserTaskMap);
        }

        // GET: GardenUserTaskMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUserTaskMap = await _context.GardenUserTaskMap.FindAsync(id);
            if (gardenUserTaskMap == null)
            {
                return NotFound();
            }
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenUserTaskMap.GardenTaskId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenUserTaskMap.GardenUserId);
            return View(gardenUserTaskMap);
        }

        // POST: GardenUserTaskMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GardenUserId,GardenTaskId")] GardenUserTaskMap gardenUserTaskMap)
        {
            if (id != gardenUserTaskMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenUserTaskMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenUserTaskMapExists(gardenUserTaskMap.Id))
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
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenUserTaskMap.GardenTaskId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenUserTaskMap.GardenUserId);
            return View(gardenUserTaskMap);
        }

        // GET: GardenUserTaskMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUserTaskMap = await _context.GardenUserTaskMap
                .Include(g => g.GardenTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenUserTaskMap == null)
            {
                return NotFound();
            }

            return View(gardenUserTaskMap);
        }

        // POST: GardenUserTaskMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenUserTaskMap = await _context.GardenUserTaskMap.FindAsync(id);
            _context.GardenUserTaskMap.Remove(gardenUserTaskMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenUserTaskMapExists(int id)
        {
            return _context.GardenUserTaskMap.Any(e => e.Id == id);
        }
    }
}
