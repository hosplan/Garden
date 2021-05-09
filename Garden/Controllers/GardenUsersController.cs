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
using System.Text;

namespace Garden.Controllers
{
    public class GardenUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGardenHelper _gardenHelper;

        public GardenUsersController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
        }

        // GET: GardenUsers
        public async Task<IActionResult> Index(int? search_gardenSpace_id)
        {
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            var myRole = _context.UserRoles.AsNoTracking().Where(z => z.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
            ApplicationRole adminRole = _context.Roles.AsNoTracking().FirstOrDefault(z => z.Name == "Admin");

            List<GardenSpace> gardenSpace_list = new List<GardenSpace>();
            //관리자인 경우
            if(myRole.FirstOrDefault(z => z.RoleId == adminRole.Id) != null)
            {
                gardenSpace_list = await _context.GardenSpace.AsNoTracking().ToListAsync();
            }
            else
            {
                gardenSpace_list = await _context.GardenSpace
                                                .Include(z => z.GardenUsers)
                                                .AsNoTracking()
                                                .Where(z => z.GardenUsers.Any(z => z.UserId == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
                                                .ToListAsync();               
            }

            if (search_gardenSpace_id == null || search_gardenSpace_id == 0)
            {                           
                search_gardenSpace_id = gardenSpace_list.First().Id;
            }

            ViewData["Garden_list"] = new SelectList(gardenSpace_list, "Id", "Name", search_gardenSpace_id);

            //var applicationDbContext = _context.GardenUser.Include(g => g.GardenSpace).Include(g => g.User);
            return View();
        }

        /// <summary>
        /// 정원 관리자 목록 가져오기
        /// </summary>
        /// <param name="id">정원id</param>
        /// <returns></returns>
        public async Task<JsonResult> GetGardenUserList(int? id)
        {
            List<object> object_list = new List<object>();

            if (id == null)
            {
                var jsonNullValue = new { data = object_list };
                return Json(jsonNullValue);
            }

            List<GardenUser> gardenUser_list = await _context.GardenUser
                                                             .Include(gUser => gUser.GardenRole)
                                                                .ThenInclude(gRole => gRole.BaseSubType)
                                                             .Include(gUser => gUser.User)
                                                             .AsNoTracking()
                                                             .Where(gUser => gUser.GardenSpaceId == id)
                                                             .ToListAsync();

            foreach(GardenUser gardenUser in gardenUser_list)
            {
                object_list.Add(new
                {
                    roleType = gardenUser.GardenRole.BaseSubType.Name,
                    userName = gardenUser.User.UserName,
                    name = gardenUser.User.Name,
                    regDate = gardenUser.CreateDate.ToShortDateString(),
                    isActive = gardenUser.IsActivate,
                    id = gardenUser.Id,
                });
            }

            var jsonValue = new { data = object_list };
            return Json(jsonValue);
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
