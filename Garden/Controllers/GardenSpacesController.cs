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
using System.Text;
using Garden.Services;

namespace Garden.Controllers
{
    public class GardenSpacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGardenHelper _gardenHelper;
        private readonly IGardenSpacesService _gardenSpacesService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GlobalValueService _globalValueService;
        public GardenSpacesController(ApplicationDbContext context, 
                                        IHttpContextAccessor httpContextAccessor, 
                                        IGardenHelper gardenHelper, 
                                        GlobalValueService globalValueService,
                                        IGardenSpacesService gardenSpacesService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
            _globalValueService = globalValueService;
            _gardenSpacesService = gardenSpacesService;
        }

        // GET: GardenSpaces
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

        /// <summary>
        /// 이달의 업무 시간정보 가져오기
        /// </summary>
        /// <param name="month"></param>
        /// <param name="gardenTask_list"></param>
        /// <returns></returns>
        private async Task<List<GardenWorkTime>> GetCurrentMonthWorkTimeList(int month, List<GardenTask> gardenTask_list)
        {
            List<GardenWorkTime> gardenWorkTime_list = new List<GardenWorkTime>();

            foreach(GardenTask gardenTask in gardenTask_list)
            {
                List<GardenWorkTime> temp_gardenWorkTime = await _context.GardenWorkTime
                                                                         .AsNoTracking()
                                                                         .Where(gWorkTime => gWorkTime.GardenTaskId == gardenTask.Id
                                                                                && gWorkTime.TaskDate.Month == month)
                                                                         .ToListAsync();

                gardenWorkTime_list.AddRange(temp_gardenWorkTime);
            }
            return gardenWorkTime_list;
        }
        public async Task<JsonResult> GetGardenTaskList(int gardenSpaceId)
        {
            List<GardenWorkTime> gardenWorkTime_list = await _context.GardenWorkTime
                                                                 .Include(gWorkTime => gWorkTime.GardenUser)
                                                                    .ThenInclude(gUser => gUser.User)
                                                                 .Include(gWorkTime => gWorkTime.GardenTask)
                                                                 .AsNoTracking()
                                                                 .Where(gWorkTime => gWorkTime.GardenSpaceId == gardenSpaceId)
                                                                 .ToListAsync();
        
            List<object> object_list = new List<object>();
            // clasName = "bg-info border-info",
            //url = "/ZWorkItems/Details/" + item.ZWorkItemId

            if(_globalValueService.IsActiveMembership)
            {
                foreach (GardenWorkTime gardenWorkTime in gardenWorkTime_list)
                {
                    object_list.Add(new
                    {
                        title = gardenWorkTime.GardenUser.User.Name,
                        start = (gardenWorkTime.TaskDate + gardenWorkTime.StartTime).ToString("s"),
                        end = (gardenWorkTime.TaskDate + gardenWorkTime.EndTime).ToString("s"),
                        className = gardenWorkTime.IsComplete == true ? "bg-info text-white worktimeInfo" : "bg-primary text-white worktimeInfo",
                        url = "/GardenWorkTimes/CompleteForWorkItem?id=" + gardenWorkTime.Id + "&spaceId=" + gardenSpaceId + "",
                    });
                }
            }
            else
            {
                foreach (GardenWorkTime gardenWorkTime in gardenWorkTime_list)
                {
                    object_list.Add(new
                    {
                        title = gardenWorkTime.GardenUser.Name,
                        start = (gardenWorkTime.TaskDate + gardenWorkTime.StartTime).ToString("s"),
                        end = (gardenWorkTime.TaskDate + gardenWorkTime.EndTime).ToString("s"),
                        className = gardenWorkTime.IsComplete == true ? "bg-info text-white worktimeInfo" : "bg-primary text-white worktimeInfo",
                        url = "/GardenWorkTimes/CompleteForWorkItem?id=" + gardenWorkTime.Id + "&spaceId=" + gardenSpaceId + "",
                    });
                }
            }
           
            var jsonValue = object_list;
            return Json(jsonValue);
        }

        public async Task<JsonResult> GetGardenEventCount()
        {
            try
            {
                GardenSpace garden = _context.GardenSpace.FirstOrDefault();

                if (garden == null)
                {
                    return new JsonResult("-");
                }
                else
                {
                   
                    List<GardenUser> anniversary_gardenUser_list = await _context.GardenUser
                                                                                .AsNoTracking()
                                                                                .Where(gUser => gUser.GardenSpaceId == garden.Id
                                                                                    && gUser.IsActivate == true
                                                                                    && gUser.IsActiveDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                                                                                .ToListAsync();


                    List<GardenUser> birthday_gardenUser_list = await _context.GardenUser
                                                                            .AsNoTracking()
                                                                            .Where(gUser => gUser.GardenSpaceId == garden.Id
                                                                                    && gUser.IsActivate == true
                                                                                    && gUser.BirthDay.Value.ToShortDateString() == DateTime.Now.ToShortDateString())
                                                                            .ToListAsync();

                    return new JsonResult(anniversary_gardenUser_list.Count() +
                                                          " / " +
                                                          birthday_gardenUser_list.Count());
                }
            }
            catch
            {
                return new JsonResult("-");
            }
        }
        public async Task<JsonResult> GetGardenPeopleCount()
        {
            try
            {
                GardenSpace garden = _context.GardenSpace.FirstOrDefault();

                if (garden == null)
                {
                    return new JsonResult("-");
                }
                else
                {
                    List<GardenUser> gardenUser_list = await _context.GardenUser
                                                                    .AsNoTracking()
                                                                    .Where(gUser => gUser.GardenSpaceId == garden.Id
                                                                           && gUser.IsActivate == true)
                                                                    .ToListAsync();


                    return new JsonResult(gardenUser_list.Count());
                }                                                        
            }
            catch
            {
                return new JsonResult("-");
            }
        }

        public async Task<JsonResult> GetGardenSpaceOtherInfo(int? gardenSpaceId)
        {
            try
            {
                if(gardenSpaceId == null)
                {
                    GardenSpace garden = _context.GardenSpace.FirstOrDefault();

                    if (garden == null)
                        return new JsonResult("-");
                    else
                        gardenSpaceId = garden.Id;
                }
                //이달 정보 가져오기
                int month = DateTime.Now.Month;

                List<GardenTask> gardenTask_list = _context.GardenTask
                                                          .AsNoTracking()
                                                          .Where(gTask => gTask.GardenSpaceId == gardenSpaceId).ToList();

                List<GardenWorkTime> currentMonth_gardenWorkTime_list = await GetCurrentMonthWorkTimeList(month, gardenTask_list);

                return new JsonResult(currentMonth_gardenWorkTime_list.Where(z => z.IsComplete == true).Count() +
                                                          " / " +
                                                          currentMonth_gardenWorkTime_list.Count());
            }
            catch
            {
                return new JsonResult(false);
            }
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

                bool isSuccess = _gardenSpacesService.CreateResource(gardenSpace);
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                //정원의 역할 생성
                _gardenHelper.CreateAllGardenRole(gardenSpace.Id);
                int gardenRole_id = _gardenHelper.GetGardenRole(gardenSpace.Id, "GARDEN_MANAGER_ROLE_TYPE_1");
                
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
                    _gardenSpacesService.UpdateResource(gardenSpace);
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
            _gardenSpacesService.RemoveResource(gardenSpace);

            return RedirectToAction(nameof(Index));
        }

        private bool GardenSpaceExists(int id)
        {
            return _context.GardenSpace.Any(e => e.Id == id);
        }
    }
}
