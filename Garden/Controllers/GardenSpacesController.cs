using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garden.Data;
using Garden.Models;
using Garden.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Garden.Controllers
{
    public class GardenSpacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGardenHelper _gardenHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GardenSpacesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
        }

        // GET: GardenSpaces
        public async Task<IActionResult> Index()
        {
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            List<GardenSpace>applicationDbContext = await _context.GardenSpace
                                                                   .Include(gardenSpace => gardenSpace.BaseSubType)
                                                                   .Include(gardenSpace => gardenSpace.GardenUsers)
                                                                   .Include(gardenSpace => gardenSpace.GardenTasks)
                                                                   .AsNoTracking()
                                                                   .ToListAsync();

            return View(applicationDbContext);
        }

        // GET: GardenSpaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var gardenSpace = await _context.GardenSpace
                                            .Include(gSpace => gSpace.BaseSubType)
                                            .Include(gSpace => gSpace.GardenUsers)
                                            .Include(gSpace => gSpace.GardenTasks)
                                            .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenSpace == null)
            {
                return NotFound();
            }

            return View(gardenSpace);
        }

        // GET: GardenSpaces/Create
        public IActionResult Create()
        {
            // 1 - 정원타입
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType.AsNoTracking().Where(z => z.BaseTypeId == "GARDEN_TYPE").ToList(), "Id", "Name");
            return PartialView();
        }

        // POST: GardenSpaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SubTypeId,CreatedDate,IsActivate")] GardenSpace gardenSpace)
        {
            if (ModelState.IsValid)
            {
                gardenSpace.IsActivate = true;
                gardenSpace.CreatedDate = DateTime.Now;
                
                _context.Add(gardenSpace);
                await _context.SaveChangesAsync();

                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                int gardenRole_id = _gardenHelper.CreateGardenRole(gardenSpace.Id, "GARDEN_MANAGER_ROLE_TYPE_1");
                
                if(gardenRole_id == 0)
                {
                    //에러메세지로. 추후 추가
                }
                //정원 생성자는 마스터 정원사로 지정 된다.
                bool returnValue = _gardenHelper.CreateGardenUser(gardenSpace.Id, userId, gardenRole_id);

                if(returnValue == false)
                {
                    //에러메세지로. 추후 추가
                }
            }
            //ViewData["SubTypeId"] = new SelectList(_context.BaseSubType.AsNoTracking().Where(z => z.BaseTypeId == "GARDEN_TYPE").ToList(), "Id", "Name");
            return RedirectToAction("Index");
            //return PartialView();
        }

        // GET: GardenSpaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSpace = await _context.GardenSpace.FindAsync(id);
            if (gardenSpace == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenSpace.SubTypeId);
            return View(gardenSpace);
        }

        // POST: GardenSpaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SubTypeId,CreatedDate,IsActivate")] GardenSpace gardenSpace)
        {
            if (id != gardenSpace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenSpace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenSpaceExists(gardenSpace.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenSpace.SubTypeId);
            return View(gardenSpace);
        }

        // GET: GardenSpaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenSpace = await _context.GardenSpace
                .Include(g => g.BaseSubType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenSpace == null)
            {
                return NotFound();
            }

            return View(gardenSpace);
        }

        // POST: GardenSpaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenSpace = await _context.GardenSpace.FindAsync(id);
            _context.GardenSpace.Remove(gardenSpace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenSpaceExists(int id)
        {
            return _context.GardenSpace.Any(e => e.Id == id);
        }
    }
}
