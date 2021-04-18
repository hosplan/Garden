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
    public class GardenUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenUsers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenUser.Include(g => g.GardenSpace).Include(g => g.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUser = await _context.GardenUser
                .Include(g => g.GardenSpace)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenUser == null)
            {
                return NotFound();
            }

            return View(gardenUser);
        }

        // GET: GardenUsers/Create
        public IActionResult Create()
        {
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: GardenUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,IsActivate,GardenSpaceId,CreateDate")] GardenUser gardenUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenUser.GardenSpaceId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", gardenUser.UserId);
            return View(gardenUser);
        }

        // GET: GardenUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUser = await _context.GardenUser.FindAsync(id);
            if (gardenUser == null)
            {
                return NotFound();
            }
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenUser.GardenSpaceId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", gardenUser.UserId);
            return View(gardenUser);
        }

        // POST: GardenUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,IsActivate,GardenSpaceId,CreateDate")] GardenUser gardenUser)
        {
            if (id != gardenUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenUserExists(gardenUser.Id))
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
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenUser.GardenSpaceId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", gardenUser.UserId);
            return View(gardenUser);
        }

        // GET: GardenUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUser = await _context.GardenUser
                .Include(g => g.GardenSpace)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenUser == null)
            {
                return NotFound();
            }

            return View(gardenUser);
        }

        // POST: GardenUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenUser = await _context.GardenUser.FindAsync(id);
            _context.GardenUser.Remove(gardenUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenUserExists(int id)
        {
            return _context.GardenUser.Any(e => e.Id == id);
        }
    }
}
