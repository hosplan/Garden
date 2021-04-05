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
    public class BaseTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BaseTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.BaseType.ToListAsync());
        }

        // GET: BaseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseType = await _context.BaseType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseType == null)
            {
                return NotFound();
            }

            return View(baseType);
        }

        // GET: BaseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BaseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsSubTypeEditable")] BaseType baseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baseType);
        }

        // GET: BaseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseType = await _context.BaseType.FindAsync(id);
            if (baseType == null)
            {
                return NotFound();
            }
            return View(baseType);
        }

        // POST: BaseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsSubTypeEditable")] BaseType baseType)
        {
            if (id != baseType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseTypeExists(baseType.Id))
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
            return View(baseType);
        }

        // GET: BaseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseType = await _context.BaseType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseType == null)
            {
                return NotFound();
            }

            return View(baseType);
        }

        // POST: BaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baseType = await _context.BaseType.FindAsync(id);
            _context.BaseType.Remove(baseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaseTypeExists(int id)
        {
            return _context.BaseType.Any(e => e.Id == id);
        }
    }
}
