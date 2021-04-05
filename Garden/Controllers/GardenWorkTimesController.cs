﻿using System;
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
    public class GardenWorkTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenWorkTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenWorkTimes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenWorkTime.Include(g => g.BaseSubType).Include(g => g.GardenTask);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenWorkTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenWorkTime = await _context.GardenWorkTime
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenWorkTime == null)
            {
                return NotFound();
            }

            return View(gardenWorkTime);
        }

        // GET: GardenWorkTimes/Create
        public IActionResult Create()
        {
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id");
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            return View();
        }

        // POST: GardenWorkTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkingTime,GardenTaskId,IsMon,IsTue,IsWed,IsThu,IsFri,IsSat,IsSun,SubTypeId,CreateDate")] GardenWorkTime gardenWorkTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenWorkTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenWorkTime.SubTypeId);
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenWorkTime.GardenTaskId);
            return View(gardenWorkTime);
        }

        // GET: GardenWorkTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenWorkTime = await _context.GardenWorkTime.FindAsync(id);
            if (gardenWorkTime == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenWorkTime.SubTypeId);
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenWorkTime.GardenTaskId);
            return View(gardenWorkTime);
        }

        // POST: GardenWorkTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WorkingTime,GardenTaskId,IsMon,IsTue,IsWed,IsThu,IsFri,IsSat,IsSun,SubTypeId,CreateDate")] GardenWorkTime gardenWorkTime)
        {
            if (id != gardenWorkTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenWorkTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenWorkTimeExists(gardenWorkTime.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenWorkTime.SubTypeId);
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenWorkTime.GardenTaskId);
            return View(gardenWorkTime);
        }

        // GET: GardenWorkTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenWorkTime = await _context.GardenWorkTime
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenWorkTime == null)
            {
                return NotFound();
            }

            return View(gardenWorkTime);
        }

        // POST: GardenWorkTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenWorkTime = await _context.GardenWorkTime.FindAsync(id);
            _context.GardenWorkTime.Remove(gardenWorkTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenWorkTimeExists(int id)
        {
            return _context.GardenWorkTime.Any(e => e.Id == id);
        }
    }
}
