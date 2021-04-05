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
    public class BaseSubTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseSubTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BaseSubTypes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BaseSubType.Include(b => b.BaseType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BaseSubTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseSubType = await _context.BaseSubType
                .Include(b => b.BaseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseSubType == null)
            {
                return NotFound();
            }

            return View(baseSubType);
        }

        // GET: BaseSubTypes/Create
        public IActionResult Create()
        {
            ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Id");
            return View();
        }

        // POST: BaseSubTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BaseTypeId,Name,Description")] BaseSubType baseSubType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baseSubType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Id", baseSubType.BaseTypeId);
            return View(baseSubType);
        }

        // GET: BaseSubTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseSubType = await _context.BaseSubType.FindAsync(id);
            if (baseSubType == null)
            {
                return NotFound();
            }
            ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Id", baseSubType.BaseTypeId);
            return View(baseSubType);
        }

        // POST: BaseSubTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BaseTypeId,Name,Description")] BaseSubType baseSubType)
        {
            if (id != baseSubType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseSubType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseSubTypeExists(baseSubType.Id))
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
            ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Id", baseSubType.BaseTypeId);
            return View(baseSubType);
        }

        // GET: BaseSubTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseSubType = await _context.BaseSubType
                .Include(b => b.BaseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseSubType == null)
            {
                return NotFound();
            }

            return View(baseSubType);
        }

        // POST: BaseSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baseSubType = await _context.BaseSubType.FindAsync(id);
            _context.BaseSubType.Remove(baseSubType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaseSubTypeExists(int id)
        {
            return _context.BaseSubType.Any(e => e.Id == id);
        }
    }
}
