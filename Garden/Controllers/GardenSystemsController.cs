using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garden.Data;
using Garden.Models;
using Microsoft.AspNetCore.Http;
using Garden.Helper;
using System.Security.Claims;

namespace Garden.Controllers
{
    public class GardenSystemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGardenHelper _gardenHelper;

        public GardenSystemsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
        }

        // GET: GardenSystems
        public async Task<IActionResult> Index()
        {
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            List<GardenSystem> system_list = await _context.GardenSystem.ToListAsync();

            if (system_list.Count() == 0)
                return RedirectToAction("Create", "GardenSystems");

            return View(await _context.GardenSystem.FirstAsync());
        }

        // GET: GardenSystems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSystem = await _context.GardenSystem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenSystem == null)
            {
                return NotFound();
            }

            return View(gardenSystem);
        }

        // GET: GardenSystems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GardenSystems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SysName,License,IsActive,SysLogo,CreateDate,ActiveMembership")] GardenSystem gardenSystem)
        {
            if (ModelState.IsValid)
            {
                gardenSystem.CreateDate = DateTime.Now;

                List<GardenSystem> exist_garden_system = _context.GardenSystem.ToList();

                if(exist_garden_system.Count() > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Add(gardenSystem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(gardenSystem);
        }

        // GET: GardenSystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSystem = await _context.GardenSystem.FindAsync(id);
            if (gardenSystem == null)
            {
                return NotFound();
            }
            return View(gardenSystem);
        }

        // POST: GardenSystems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SysName,License,IsActive,SysLogo,CreateDate,ActiveMembership")] GardenSystem gardenSystem)
        {
            if (id != gardenSystem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenSystem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenSystemExists(gardenSystem.Id))
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
            return View(gardenSystem);
        }

        // GET: GardenSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSystem = await _context.GardenSystem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenSystem == null)
            {
                return NotFound();
            }

            return View(gardenSystem);
        }

        // POST: GardenSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenSystem = await _context.GardenSystem.FindAsync(id);
            _context.GardenSystem.Remove(gardenSystem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenSystemExists(int id)
        {
            return _context.GardenSystem.Any(e => e.Id == id);
        }
    }
}
