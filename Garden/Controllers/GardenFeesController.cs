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
using Garden.Services;

namespace Garden.Controllers
{
    public class GardenFeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGardenHelper _gardenHelper;
        private readonly GlobalValueService _globalValueService;
        private readonly IGardenService _gardenService;
        public GardenFeesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper, GlobalValueService globalValueService, IGardenService gardenService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
            _globalValueService = globalValueService;
            _gardenService = gardenService;
        }

        // GET: GardenFees
        public async Task<IActionResult> Index()
        {
            //check lisense
            bool isActiveSystem = _globalValueService.SystemStatus;

            if (!isActiveSystem)
                return RedirectToAction("NoLicense", "Home");
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            var applicationDbContext = _context.GardenFee.Include(g => g.BaseSubType).Include(g => g.GardenSpace).Include(g => g.GardenUser);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// GardenUser의 정보 가져오기
        /// GardenFee(회비) 포함 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<List<GardenUser>> GetGardenUserFees(int id)
        {
            List<GardenUser> gardenUsers = new List<GardenUser>();

            try
            {
                gardenUsers = await _context.GardenUser
                                            .Include(gardenUser => gardenUser.GardenFees)
                                            .AsNoTracking()
                                            .Where(gardenUser => gardenUser.GardenSpaceId == id)
                                            .ToListAsync();
                return gardenUsers;
            }
            catch
            {
                return gardenUsers;
            }
        }

        private List<GardenUser> GetGardenUserFeeForUser(List<GardenUser> gardenUsers, int gardenUserId)
        {
            try
            {
                gardenUsers = gardenUsers.Where(gardenUser => gardenUser.Id == gardenUserId).ToList();
                return gardenUsers;
            }
            catch
            {
                return gardenUsers;
            }
        }

        /// <summary>
        /// 시작 날짜.
        /// </summary>
        /// <param name="gardenUsers"></param>
        /// <param name="startMonth"></param>
        /// <returns></returns>
        private List<GardenUser> GetGardenUserFeeForStartMonth(List<GardenUser> gardenUsers, int startMonth)
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                gardenUsers = gardenUsers.Where(gardenUser => gardenUser.CreateDate.Year == currentYear &&
                                                              gardenUser.CreateDate.Month > startMonth)
                                         .ToList();

                return gardenUsers;
            }
            catch
            {
                return gardenUsers;
            }
        }

        private async Task CreateCurrentGardenFee(List<GardenFee> gardenFees, int gardenSpaceId)
        {
            List<GardenUser> gardenUsers = await _context.GardenUser
                                                        .AsNoTracking()
                                                        .Where(gardenUser => gardenUser.GardenSpaceId == gardenSpaceId && gardenUser.IsActivate == true)
                                                        .ToListAsync();

            foreach (GardenFee gardenFee in gardenFees)
            {
                GardenUser gardenUser = await _context.GardenUser.AsNoTracking().FirstOrDefaultAsync(gardenUser => gardenUser.Id == gardenFee.GardenUserId);
                gardenUsers.Remove(gardenUser);
            }

            List<GardenFee> createGardenFees = new List<GardenFee>();
            foreach(GardenUser gardenUser in gardenUsers)
            {
                GardenFee gardenFee = new GardenFee();

                gardenFee.GardenUserId = gardenUser.Id;
                gardenFee.GardenSpaceId = gardenSpaceId;
                gardenFee.CreateDate = DateTime.Now;
                gardenFee.Amount = 0;

                createGardenFees.Add(gardenFee);
            }

            _context.AddRange(createGardenFees);
            await _context.SaveChangesAsync();
        }

        private async Task<List<GardenFee>> GetExistCurrentGardenFee(int gardenSpaceId)
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            List<GardenFee> gardenFees = await _context.GardenFee
                                                       .AsNoTracking()
                                                       .Where(gardenFee => gardenFee.CreateDate.Year == currentYear &&
                                                                           gardenFee.CreateDate.Month == currentMonth &&
                                                                           gardenFee.GardenSpaceId == gardenSpaceId)
                                                       .ToListAsync();

            return gardenFees;
        }

        /// <summary>
        /// GardenFee 정보 가져오기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetGardenFeeJsonList(int id, int? gardenUserId, int? startMonth, int? endMonth)
        {
            List<object> object_list = new List<object>();
            try
            {
                List<GardenFee> existGardenFees = await GetExistCurrentGardenFee(id);
                //await CreateCurrentGardenFee(existGardenFees, id);
               
                //if(gardenUserId != null)
                //{
                //    gardenUsers = GetGardenUserFeeForUser(gardenUsers, gardenUserId.Value);
                //}
                //if(startMonth != null)
                //{
                //    gardenUsers = GetGardenUserFeeForStartMonth(gardenUsers, startMonth.Value);
                //}
                //if(endMonth != null)
                //{
                //    gardenUsers = GetGardenUserFeeForEndMonth(gardenUsers, endMonth.Value);
                //}
                
                

                var jsonValue = object_list;
                return Json(jsonValue);
            }
            catch
            {
                var jsonValue = object_list;
                return Json(jsonValue);
            }
        }
        public async Task<IActionResult> IndexForFeeType()
        {

            return View();
        }

        // GET: GardenFees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //check lisense
            bool isActiveSystem = _globalValueService.SystemStatus;

            if (!isActiveSystem)
                return RedirectToAction("NoLicense", "Home");
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

            var gardenFee = await _context.GardenFee
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenSpace)
                .Include(g => g.GardenUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenFee == null)
            {
                return NotFound();
            }

            return View(gardenFee);
        }

        // GET: GardenFees/Create
        public IActionResult Create()
        {
            //check lisense
            bool isActiveSystem = _globalValueService.SystemStatus;

            if (!isActiveSystem)
                return RedirectToAction("NoLicense", "Home");
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id");
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id");
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id");
            return View();
        }

        // POST: GardenFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubTypeId,Amount,CreateDate,GardenUserId,GardenSpaceId")] GardenFee gardenFee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenFee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenFee.SubTypeId);
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenFee.GardenSpaceId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenFee.GardenUserId);
            return View(gardenFee);
        }

        // GET: GardenFees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //check lisense
            bool isActiveSystem = _globalValueService.SystemStatus;

            if (!isActiveSystem)
                return RedirectToAction("NoLicense", "Home");
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

            var gardenFee = await _context.GardenFee.FindAsync(id);
            if (gardenFee == null)
            {
                return NotFound();
            }
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenFee.SubTypeId);
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenFee.GardenSpaceId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenFee.GardenUserId);
            return View(gardenFee);
        }

        // POST: GardenFees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubTypeId,Amount,CreateDate,GardenUserId,GardenSpaceId")] GardenFee gardenFee)
        {
            if (id != gardenFee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenFee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenFeeExists(gardenFee.Id))
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
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType, "Id", "Id", gardenFee.SubTypeId);
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenFee.GardenSpaceId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenFee.GardenUserId);
            return View(gardenFee);
        }

        // GET: GardenFees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenFee = await _context.GardenFee
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenSpace)
                .Include(g => g.GardenUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenFee == null)
            {
                return NotFound();
            }

            return View(gardenFee);
        }

        // POST: GardenFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenFee = await _context.GardenFee.FindAsync(id);
            _context.GardenFee.Remove(gardenFee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenFeeExists(int id)
        {
            return _context.GardenFee.Any(e => e.Id == id);
        }
    }
}
