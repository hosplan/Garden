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
            try
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
                if (myRole.FirstOrDefault(z => z.RoleId == adminRole.Id) != null)
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

                if ((search_gardenSpace_id == null || search_gardenSpace_id == 0) && gardenSpace_list.Count() != 0)
                    search_gardenSpace_id = gardenSpace_list.First().Id;
                else
                    search_gardenSpace_id = 0;

                ViewData["Garden_list"] = new SelectList(gardenSpace_list, "Id", "Name", search_gardenSpace_id);

            }
            catch
            {
               
            }
           
            //var applicationDbContext = _context.GardenUser.Include(g => g.GardenSpace).Include(g => g.User);
            return View();
        }

        /// <summary>
        /// 정원에 참여시키는 모달창 열기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetActiveUserList(int? id)
        {
            List<object> object_list = new List<object>();

            try
            {
                List<ApplicationUser> activeUser_list = await _context.Users
                                                                       .Where(user => user.IsActive == true && 
                                                                              user.UserName != "SYSTEM").ToListAsync();

                List<GardenUser> gardenUser_list = await _context.GardenUser
                                                                 .Include(gUser => gUser.User)
                                                                 .Where(gUser => gUser.GardenSpaceId == id).ToListAsync();

                //기존에 정원 관리사에 포함되어 있는 유저는 삭제
                foreach(GardenUser gardenUser in gardenUser_list)
                {
                    bool t = activeUser_list.Remove(gardenUser.User);
                }
                    
                
                
                foreach(ApplicationUser activeUser in activeUser_list)
                {
                    object_list.Add(new
                    {
                        id = activeUser.Id,
                        name = activeUser.Name,
                        userName = activeUser.UserName
                    });
                }
            }
            catch
            {

            }
            var jsonValue = new { data = object_list };
            return Json(jsonValue);
        }

        /// <summary>
        /// 참여시키기 혹은 빼기
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="isCheck">체크여부</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AttendForGardenUser(string userId, bool isCheck, int? gardenSpaceId, string gardenRoleId)
        {
            if (userId == null || !gardenSpaceId.HasValue)
                return new JsonResult(false);

            try
            {
                GardenUser gardenUser = await _context.GardenUser
                                                      .FirstOrDefaultAsync(gUser => gUser.UserId == userId && 
                                                                           gUser.GardenSpaceId == gardenSpaceId);
                                
                if (gardenUser == null && isCheck == true)
                {
                    int gardenRole_id = _gardenHelper.CreateGardenRole(gardenSpaceId.Value, gardenRoleId);
                    bool returnValue = _gardenHelper.CreateGardenUser(gardenSpaceId.Value, userId, gardenRole_id);

                    if (returnValue == false)
                        return new JsonResult(false);
                }
                else if(gardenUser != null && isCheck == false)
                {
                    gardenUser.IsActivate = false;

                    _context.Update(gardenUser);
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
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
        public IActionResult Create(int? id)
        {
            if (id == null)
                return NotFound();

            ViewData["roleType_list"] = new SelectList(_context.BaseSubType.Where(subType => subType.BaseTypeId == "GARDEN_MANAGER_ROLE_TYPE" && subType.Id != "GARDEN_MANAGER_ROLE_TYPE_1").ToList(), "Id", "Name");
            ViewData["gardenSpace_id"] = id;

            return PartialView();
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
        /// <summary>
        /// 정원 관리자 역할 변경 모달창 열기
        /// </summary>
        /// <param name="id">gardenUserId</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditForGardenUserRole(int? id)
        {
            GardenUser gardenUser = await _context.GardenUser
                                                  .Include(gUser => gUser.User)
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(gUser => gUser.Id == id);

            ViewData["edit_roleType_list"] = new SelectList(_context.BaseSubType.Where(subType => subType.BaseTypeId == "GARDEN_MANAGER_ROLE_TYPE" && subType.Id != "GARDEN_MANAGER_ROLE_TYPE_1").ToList(), "Id", "Name");

            return PartialView(gardenUser);
        }
        /// <summary>
        /// 정원 관리자 역할 변경
        /// </summary>
        /// <param name="id">gardenUserId</param>
        /// <param name="gardenRoleTypeId">정원 역할 id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> EditForGardenUserRole(int gardenUserId, string gardenRoleTypeId)
        {
            if(gardenUserId == 0 || string.IsNullOrEmpty(gardenRoleTypeId))
            {
                return new JsonResult(false);
            }
            
            try
            {
                GardenUser gardenUser = await _context.GardenUser.FirstOrDefaultAsync(gUser => gUser.Id == gardenUserId);
                GardenRole gardenRole = await _context.GardenRole.FirstOrDefaultAsync(gRole => gRole.SubTypeId == gardenRoleTypeId);
                
                if(gardenRole == null)
                    gardenUser.GardenRoleId = _gardenHelper.CreateGardenRole(gardenUser.GardenSpaceId.Value, gardenRoleTypeId);
                else
                    gardenUser.GardenRoleId = gardenRole.Id;

                _context.Update(gardenUser);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
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

        [HttpPost]
        public async Task<JsonResult> DeleteGardenUser(int? gardenUserId)
        {
            if (gardenUserId == null)
                return new JsonResult(false);

            GardenUser gardenUser = await _context.GardenUser.FirstOrDefaultAsync(gUser => gUser.Id == gardenUserId);

            List<GardenUserTaskMap> gardenUserTaskMap_list = await _context.GardenUserTaskMap
                                                                            .Include(gUserTaskMap => gUserTaskMap.GardenUser)
                                                                            .Include(gUserTaskMap => gUserTaskMap.GardenManager)
                                                                            .Include(gUserTaskMap => gUserTaskMap.GardenTask)
                                                                            .Include(gUserTaskMap => gUserTaskMap.GardenWorkTime)
                                                                            .Where(gUserTaskMap => gUserTaskMap.GardenManagerId == gardenUserId
                                                                                   || gUserTaskMap.GardenUserId == gardenUserId)
                                                                            .ToListAsync();

            List<GardenTask> remove_gardenTask_list = new List<GardenTask>();
            List<GardenUser> remove_gardenUser_list = new List<GardenUser>();
            List<GardenWorkTime> remove_gardenWorkItem_list = new List<GardenWorkTime>();

            foreach(GardenUserTaskMap gardenUserTaskMap in gardenUserTaskMap_list)
            {
                //자신이 담당자인경우 해당 Task는 삭제 해야된다.
                //또한 자신이 포함된 수업이 삭제 되기때문에
                //해당 수업과 연결되어 있는 수강자 map 까지 삭제 해줘야 한다.
                if(gardenUserTaskMap.GardenManagerId != null)
                {
                    remove_gardenTask_list.Add(gardenUserTaskMap.GardenTask);

                }
            }

            return new JsonResult(true);
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
