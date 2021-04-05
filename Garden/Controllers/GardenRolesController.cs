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
    public class GardenRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenRoles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenRole.Include(g => g.BaseSubType).Include(g => g.Garden);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenRole = await _context.GardenRole
                .Include(g => g.BaseSubType)
                .Include(g => g.Garden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenRole == null)
            {
                return NotFound();
            }

            return View(gardenRole);
        }

        // GET: GardenRoles/Create
        public IActionResult Create()
        {
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id");
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id");
            return View();
        }

        // POST: GardenRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GardenId,SubTypeId")] GardenRole gardenRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenRole.SubTypeId);
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id", gardenRole.GardenId);
            return View(gardenRole);
        }

        // GET: GardenRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenRole = await _context.GardenRole.FindAsync(id);
            if (gardenRole == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenRole.SubTypeId);
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id", gardenRole.GardenId);
            return View(gardenRole);
        }

        // POST: GardenRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GardenId,SubTypeId")] GardenRole gardenRole)
        {
            if (id != gardenRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenRoleExists(gardenRole.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenRole.SubTypeId);
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id", gardenRole.GardenId);
            return View(gardenRole);
        }

        // GET: GardenRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenRole = await _context.GardenRole
                .Include(g => g.BaseSubType)
                .Include(g => g.Garden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenRole == null)
            {
                return NotFound();
            }

            return View(gardenRole);
        }

        // POST: GardenRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenRole = await _context.GardenRole.FindAsync(id);
            _context.GardenRole.Remove(gardenRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenRoleExists(int id)
        {
            return _context.GardenRole.Any(e => e.Id == id);
        }
    }
}
