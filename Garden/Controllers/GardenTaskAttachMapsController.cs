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
    public class GardenTaskAttachMapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenTaskAttachMapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenTaskAttachMaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenTaskAttachMap.Include(g => g.Attachment).Include(g => g.GardenSpace);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenTaskAttachMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTaskAttachMap = await _context.GardenTaskAttachMap
                .Include(g => g.Attachment)
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenTaskAttachMap == null)
            {
                return NotFound();
            }

            return View(gardenTaskAttachMap);
        }

        // GET: GardenTaskAttachMaps/Create
        public IActionResult Create()
        {
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id");
            ViewData["GardenId"] = new SelectList(_context.GardenSpace, "Id", "Id");
            return View();
        }

        // POST: GardenTaskAttachMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GardenId,AttachmentId")] GardenTaskAttachMap gardenTaskAttachMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenTaskAttachMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id", gardenTaskAttachMap.AttachmentId);
            ViewData["GardenId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenTaskAttachMap.GardenId);
            return View(gardenTaskAttachMap);
        }

        // GET: GardenTaskAttachMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTaskAttachMap = await _context.GardenTaskAttachMap.FindAsync(id);
            if (gardenTaskAttachMap == null)
            {
                return NotFound();
            }
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id", gardenTaskAttachMap.AttachmentId);
            ViewData["GardenId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenTaskAttachMap.GardenId);
            return View(gardenTaskAttachMap);
        }

        // POST: GardenTaskAttachMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GardenId,AttachmentId")] GardenTaskAttachMap gardenTaskAttachMap)
        {
            if (id != gardenTaskAttachMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenTaskAttachMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenTaskAttachMapExists(gardenTaskAttachMap.Id))
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
            ViewData["AttachmentId"] = new SelectList(_context.Attachment, "Id", "Id", gardenTaskAttachMap.AttachmentId);
            ViewData["GardenId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenTaskAttachMap.GardenId);
            return View(gardenTaskAttachMap);
        }

        // GET: GardenTaskAttachMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTaskAttachMap = await _context.GardenTaskAttachMap
                .Include(g => g.Attachment)
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenTaskAttachMap == null)
            {
                return NotFound();
            }

            return View(gardenTaskAttachMap);
        }

        // POST: GardenTaskAttachMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenTaskAttachMap = await _context.GardenTaskAttachMap.FindAsync(id);
            _context.GardenTaskAttachMap.Remove(gardenTaskAttachMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenTaskAttachMapExists(int id)
        {
            return _context.GardenTaskAttachMap.Any(e => e.Id == id);
        }
    }
}
