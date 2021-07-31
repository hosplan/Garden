﻿using System;
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
        /// 이달 회비 납부한 회원
        /// </summary>
        /// <param name="gardenSpaceId">gardenSpaceId</param>
        /// <returns></returns>
        private async Task<List<GardenUser>> GetGardenUserRelateFee(int gardenSpaceId)
        {
            List<GardenUser> gardenUsers = new List<GardenUser>();

            try
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;

                gardenUsers = await _context.GardenUser
                                            .Include(gardenUser => gardenUser.GardenFees)
                                                .ThenInclude(gardenFee => gardenFee.BaseSubType)
                                            .Include(gardenUser => gardenUser.GardenFees)
                                                .ThenInclude(gardenFee => gardenFee.DiscountType)
                                            .AsNoTracking()
                                            .Where(gardenUser => 
                                                    gardenUser.GardenFees.Where(gardenFee => gardenFee.CreateDate.Year == currentYear &&
                                                                                             gardenFee.CreateDate.Month == currentMonth).Count() > 0 
                                                    && gardenUser.GardenSpaceId == gardenSpaceId)
                                            .ToListAsync();

                return gardenUsers;
            }
            catch
            {
                return gardenUsers;
            }
        }

        /// <summary>
        /// GardenUser의 정보 가져오기
        /// 이달 회비 납부 하지 않은 회원 
        /// </summary>
        /// <param name="gardenSpaceId">gardenSpaceId</param>
        /// <returns></returns>
        private async Task<List<GardenUser>> GetGardenUserUnRelateFee(int gardenSpaceId)
        {
            List<GardenUser> gardenUsers = new List<GardenUser>();

            try
            {
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;

                gardenUsers = await _context.GardenUser
                                           .Include(gardenUser => gardenUser.GardenFees)
                                                .ThenInclude(gardenFee => gardenFee.BaseSubType)
                                           .Include(gardenUser => gardenUser.GardenFees)
                                                .ThenInclude(gardenFee => gardenFee.DiscountType)
                                           .AsNoTracking()
                                           .Where(gardenUser => 
                                                  gardenUser.GardenFees.Where(gardenFee => gardenFee.CreateDate.Year == currentYear &&
                                                                                           gardenFee.CreateDate.Month == currentMonth).Count() == 0 
                                                  && gardenUser.GardenSpaceId == gardenSpaceId)
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
        /// GardenFee 정보 가져오기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetGardenFeeJsonList(int id, int? gardenUserId, int? startMonth, int? endMonth)
        {
            List<object> object_list = new List<object>();
            try
            {
                //이달에 회비를 납부한 인원
                List<GardenUser> gardenUsers_relate_fee = await GetGardenUserRelateFee(id);
                //이달에 회비를 납부하지 않은 인원
                List<GardenUser> gardenUsers_unRelate_fee = await GetGardenUserUnRelateFee(id);

                foreach (GardenUser gardenUser in gardenUsers_relate_fee)
                {
                    object_list.Add(new
                    {           
                        gardenId = gardenUser.GardenSpaceId,
                        userName = gardenUser.Name,
                        feeId = gardenUser.GardenFees.First().Id,
                        feeType = gardenUser.GardenFees.First().BaseSubType.Name,
                        discountType = gardenUser.GardenFees.First().DiscountType.Name,
                        createDate = gardenUser.GardenFees.First().CreateDate.ToShortDateString(),
                        expireDate = gardenUser.GardenFees.First().ExpireDate.ToShortDateString(),
                        userId = gardenUser.UserId,
                    });
                }
    
                foreach (GardenUser gardenUser in gardenUsers_unRelate_fee)
                {
                    object_list.Add(new
                    {
                        gardenId = gardenUser.GardenSpaceId,                        
                        userName = gardenUser.Name,
                        feeId = '0',
                        feeType = '-',
                        discountType = '-',
                        createDate = '-',
                        expireDate = '-',
                        userId = gardenUser.Id,
                    });
                }

                var jsonValue = new { data = object_list };
                return Json(jsonValue);
            }
            catch
            {
                var jsonValue = new { data = object_list };
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
        public IActionResult Create(int gardenId, int gardenUserId)
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

            ViewData["GardenId"] = gardenId;
            ViewData["GardenUserId"] = gardenUserId;
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType.Where(baseSubType => baseSubType.BaseTypeId == "GARDEN_FEE_TYPE"), "Id", "Name");
            ViewData["DiscountTypeId"] = new SelectList(_context.BaseSubType.Where(baseSubType => baseSubType.BaseTypeId == "GARDEN_FEE_DISCOUNT_TYPE"), "Id", "Name");

            return PartialView();
        }

        private object CalculateCreateAndExpire(string description)
        {
            string createDate = DateTime.Now.ToShortDateString();
            string expireDate = DateTime.Now.AddMonths(Convert.ToInt32(description)).ToShortDateString();

            var dateValue = new
            {
                createDate = createDate,
                expireDate = expireDate
            };

            return dateValue;
        }

        [HttpPost]
        public async Task<JsonResult> CalculateDate(string feeTypeId)
        {            
            try
            {
                BaseSubType feeType = await _context.BaseSubType.FindAsync(feeTypeId);

                if (feeType == null)
                    return new JsonResult("empty");

                return new JsonResult(CalculateCreateAndExpire(feeType.Description));
            }
            catch
            {
                return new JsonResult(false);
            }
        }

        // POST: GardenFees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubTypeId,CreateDate,ExpireDate,DisCountTypeId,GardenUserId,GardenSpaceId")] GardenFee gardenFee)
        {
            ViewData["GardenId"] = gardenFee.GardenSpaceId;
            ViewData["GardenUserId"] = gardenFee.GardenUserId;
            ViewData["SubTypeId"] = new SelectList(_context.BaseSubType.Where(baseSubType => baseSubType.BaseTypeId == "GARDEN_FEE_TYPE"), "Id", "Name");
            ViewData["DiscountTypeId"] = new SelectList(_context.BaseSubType.Where(baseSubType => baseSubType.BaseTypeId == "GARDEN_FEE_DISCOUNT_TYPE"), "Id", "Name");

            if (ModelState.IsValid)
            {
                _context.Add(gardenFee);
                await _context.SaveChangesAsync();

                return PartialView();
            }

            return PartialView();
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
