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
    public class GardenAttachMapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenAttachMapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenAttachMaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenAttachMap.Include(g => g.Attachment).Include(g => g.Garden);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenAttachMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenAttachMap = await _context.GardenAttachMap
                .Include(g => g.Attachment)
                .Include(g => g.Garden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenAttachMap == null)
            {
                return NotFound();
            }

            return View(gardenAttachMap);
        }

        // GET: GardenAttachMaps/Create
        public IActionResult Create()
        {
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id");
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id");
            return View();
        }

        // POST: GardenAttachMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GardenId,AttachmentId")] GardenAttachMap gardenAttachMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenAttachMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id", gardenAttachMap.AttachmentId);
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id", gardenAttachMap.GardenId);
            return View(gardenAttachMap);
        }

        // GET: GardenAttachMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenAttachMap = await _context.GardenAttachMap.FindAsync(id);
            if (gardenAttachMap == null)
            {
                return NotFound();
            }
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id", gardenAttachMap.AttachmentId);
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id", gardenAttachMap.GardenId);
            return View(gardenAttachMap);
        }

        // POST: GardenAttachMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GardenId,AttachmentId")] GardenAttachMap gardenAttachMap)
        {
            if (id != gardenAttachMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenAttachMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenAttachMapExists(gardenAttachMap.Id))
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
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id", gardenAttachMap.AttachmentId);
            ViewData["GardenId"] = new SelectList(_context.Set<GardenSpace>(), "Id", "Id", gardenAttachMap.GardenId);
            return View(gardenAttachMap);
        }

        // GET: GardenAttachMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenAttachMap = await _context.GardenAttachMap
                .Include(g => g.Attachment)
                .Include(g => g.Garden)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenAttachMap == null)
            {
                return NotFound();
            }

            return View(gardenAttachMap);
        }

        // POST: GardenAttachMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenAttachMap = await _context.GardenAttachMap.FindAsync(id);
            _context.GardenAttachMap.Remove(gardenAttachMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenAttachMapExists(int id)
        {
            return _context.GardenAttachMap.Any(e => e.Id == id);
        }
    }
}
