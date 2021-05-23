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
    public class GardenTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGardenHelper _gardenHelper;
        public GardenTasksController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
        }

        /// <summary>
        /// 정원 업무 참가시키기
        /// </summary>
        /// <param name="id">GardenTaskId</param>
        /// <returns></returns>
        public async Task<IActionResult> AttendGardenUser(int? id)
        {
            if (id == null)
                return NotFound();

            GardenTask gardenTask = await _context.GardenTask.FirstOrDefaultAsync(z => z.Id == id);

            ViewData["gardenSpace_id"] = gardenTask.GardenSpaceId;
        
            return PartialView(gardenTask);
        }
        public async Task<JsonResult> AttendGardenTaskManager(int gardenUserId, int gardenTaskId)
        {
            GardenUserTaskMap create_gardenUserTaskMap = new GardenUserTaskMap();

            create_gardenUserTaskMap.GardenManagerId = gardenUserId;
            create_gardenUserTaskMap.GardenTaskId = gardenTaskId;
            create_gardenUserTaskMap.RegDate = DateTime.Now;

            try
            {
                _context.Add(create_gardenUserTaskMap);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        [HttpPost]
        public async Task<JsonResult> AttendGardenTaskUser(int gardenUserId, int gardenTaskId)
        {
            GardenUserTaskMap create_gardenUserTaskMap = new GardenUserTaskMap();

            create_gardenUserTaskMap.GardenUserId = gardenUserId;
            create_gardenUserTaskMap.GardenTaskId = gardenTaskId;
            create_gardenUserTaskMap.RegDate = DateTime.Now;

            try
            {
                _context.Add(create_gardenUserTaskMap);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        /// <summary>
        /// 참여된 사람이 있는지 확인
        /// </summary>
        /// <param name="gardenUser_list"></param>
        /// <param name="gardenTaskId"></param>
        /// <returns></returns>
        private async Task<List<GardenUser>> CheckAttendingUser(List<GardenUser> gardenUser_list, int gardenTaskId)
        {
            List<GardenUserTaskMap> gardenTaskMap_list = await _context.GardenUserTaskMap
                                                               .Include(gUserTaskMap => gUserTaskMap.GardenUser)
                                                               .Include(gUserTaskMap => gUserTaskMap.GardenManager)
                                                               .Where(gUserTaskMap => gUserTaskMap.GardenTaskId == gardenTaskId)
                                                               .ToListAsync();

            foreach (GardenUserTaskMap gardenTaskMap in gardenTaskMap_list)
            {

                if (gardenUser_list.Contains(gardenTaskMap.GardenManager))
                {
                    gardenUser_list.Remove(gardenTaskMap.GardenManager);
                }
                else if (gardenUser_list.Contains(gardenTaskMap.GardenUser))
                {
                    gardenUser_list.Remove(gardenTaskMap.GardenUser);
                }
            }

            return gardenUser_list;
        }

        /// <summary>
        /// 정원 관리자 목록 가져오기
        /// </summary>
        /// <param name="id">정원id</param>
        /// <returns></returns>
        public async Task<JsonResult> LoadGardenUserList(int gardenSpaceId, int gardenTaskId)
        {
            List<object> object_list = new List<object>();

            if (gardenSpaceId == 0 || gardenTaskId == 0)
            {
                var jsonNullValue = new { data = object_list };
                return Json(jsonNullValue);
            }

            List<GardenUser> gardenUser_list = await _context.GardenUser
                                                             .Include(gUser => gUser.User)                                                            
                                                             .Where(gUser => gUser.GardenSpaceId == gardenSpaceId && gUser.GardenRole.SubTypeId != "GARDEN_MANAGER_ROLE_TYPE_1")
                                                             .ToListAsync();
            //이미 참여된 사람이 있는지 확인
            gardenUser_list = await CheckAttendingUser(gardenUser_list, gardenTaskId);
 
            foreach (GardenUser gardenUser in gardenUser_list)
            {
                object_list.Add(new
                {
                    //roleType = gardenUser.GardenRole.BaseSubType.Name,
                    userName = gardenUser.User.UserName,
                    name = gardenUser.User.Name,
                    regDate = gardenUser.CreateDate.ToShortDateString(),
                    id = gardenUser.Id,
                });
            }

            var jsonValue = new { data = object_list };
            return Json(jsonValue);
        }

        // GET: GardenTasks
        public async Task<IActionResult> Index(int? search_gardenSpace_id)
        {
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            if (search_gardenSpace_id == null || search_gardenSpace_id == 0)
            {
                GardenSpace gardenSpace = _context.GardenSpace.FirstOrDefault();
                
                if(gardenSpace != null)                
                    search_gardenSpace_id = gardenSpace.Id;                              
            }

            ViewData["Garden_list"] = new SelectList(_context.GardenSpace, "Id", "Name", search_gardenSpace_id);
            var applicationDbContext = _context.GardenTask.Include(g => g.BaseSubType).Include(g => g.GardenSpace);
            return View(await applicationDbContext.ToListAsync());
        }

        public JsonResult GetGardenTaskList(int id)
        {
            List<object> returnValue_object_list = new List<object>();
          

            if (id == 0)
            {
                return Json(returnValue_object_list);
            }

            List<GardenTask> gardenTask_list = _context.GardenTask
                                                       .Include(gardenTask => gardenTask.BaseSubType)
                                                       .Include(gardenTask => gardenTask.GardenUserTaskMaps)
                                                       .AsNoTracking()
                                                       .Where(gardenTask => gardenTask.GardenSpaceId == id)
                                                       .OrderBy(gardenTask => gardenTask.CreateDate)
                                                       .ToList();

            foreach(GardenTask gardenTask in gardenTask_list)
            {
                returnValue_object_list.Add(new
                {
                    id = gardenTask.Id,
                    typeName = gardenTask.BaseSubType.Name,
                    name = gardenTask.Name,
                    userCount = gardenTask.GardenUserTaskMaps.Count(),
                    todayTask = 0,
                    isActive = gardenTask.IsActivate,                    
                    createDate = gardenTask.CreateDate.ToShortDateString(),
                });
            }

            var jsonResult = new { data = returnValue_object_list };
            return Json(jsonResult);
        }

        // GET: GardenTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                         this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                         this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var gardenTask = await _context.GardenTask
                                            .Include(gTask => gTask.BaseSubType)
                                            .Include(gTask => gTask.GardenSpace)
                                            .Include(gTask => gTask.GardenUserTaskMaps)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(gTask => gTask.Id == id);
            if (gardenTask == null)
            {
                return NotFound();
            }

            return View(gardenTask);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GardenSpaceId</param>
        /// <returns></returns>
        // GET: GardenTasks/Create
        public IActionResult Create(int id)
        {
            GardenSpace gardenSpace = _context.GardenSpace.FirstOrDefault(z => z.Id == id);
            if(gardenSpace == null)
            {
                return PartialView();
            }
            ViewData["gardenSpace_name"] = gardenSpace.Name;
            ViewData["gardenSpace_id"] = id;
            ViewData["subType_id"] = new SelectList(_context.BaseSubType.Where(sType => sType.BaseTypeId == "GARDEN_TASK_TIME_TYPE"), "Id", "Name");
            return PartialView();
        }

        // POST: GardenTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,SubTypeId,IsActivate,CreateDate,GardenSpaceId")] GardenTask gardenTask)
        {
            if (ModelState.IsValid)
            {
                gardenTask.IsActivate = true;
                gardenTask.CreateDate = DateTime.Now;
                gardenTask.RegUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(gardenTask);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { search_gardenSpace_id = gardenTask.GardenSpaceId });
            }

            GardenSpace gardenSpace = _context.GardenSpace.FirstOrDefault(z => z.Id == gardenTask.GardenSpaceId);
            ViewData["gardenSpace_name"] = gardenSpace.Name;
            ViewData["gardenSpace_id"] = gardenTask.GardenSpaceId;
            ViewData["subType_id"] = new SelectList(_context.BaseSubType, "Id", "Name");
            return PartialView();
        }

        // GET: GardenTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTask = await _context.GardenTask.FindAsync(id);
            if (gardenTask == null)
            {
                return NotFound();
            }

            ViewData["subType_id"] = new SelectList(_context.BaseSubType.Where(sType => sType.BaseTypeId == "GARDEN_TASK_TIME_TYPE"), "Id", "Name", gardenTask.SubTypeId);

            return PartialView(gardenTask);
        }

        // POST: GardenTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,Description,SubTypeId,IsActivate,CreateDate,GardenSpaceId")] GardenTask gardenTask)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenTaskExists(gardenTask.Id))
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
            ViewData["subType_id"] = new SelectList(_context.BaseSubType.Where(sType => sType.BaseTypeId == "GARDEN_TASK_TIME_TYPE"), "Id", "Name", gardenTask.SubTypeId);

            return PartialView(gardenTask);
        }

        // GET: GardenTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenTask = await _context.GardenTask
                .Include(g => g.BaseSubType)
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenTask == null)
            {
                return NotFound();
            }

            return View(gardenTask);
        }

        // POST: GardenTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenTask = await _context.GardenTask.FindAsync(id);
            _context.GardenTask.Remove(gardenTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenTaskExists(int id)
        {
            return _context.GardenTask.Any(e => e.Id == id);
        }
    }
}
