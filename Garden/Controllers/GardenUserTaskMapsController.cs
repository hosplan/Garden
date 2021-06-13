using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garden.Data;
using Garden.Models;
using System.Text;
using Garden.Helper;

namespace Garden.Controllers
{
    public class GardenUserTaskMapsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalValueService _globalValueService;
        public GardenUserTaskMapsController(ApplicationDbContext context, GlobalValueService globalValueService)
        {
            _context = context;
            _globalValueService = globalValueService;
        }

        public async Task<JsonResult> ChangeRentStatus(int gardenUserTaskMapId, bool isRent)
        {
            GardenUserTaskMap map = _context.GardenUserTaskMap.FirstOrDefault(map => map.Id == gardenUserTaskMapId);

            if (map == null)
                return new JsonResult(false);

            map.IsRent = isRent;

            try
            {
                _context.Update(map);
                await _context.SaveChangesAsync();
                return new JsonResult(true);

            }
            catch
            {
                return new JsonResult(false);
            }           
        }

        //참여자의 업무시간 목록 가져오기
        public async Task<JsonResult> GetGardenUserWorkTime(int gardenUserTaskMapId, int startMonth, int endMonth)
        {
            if(gardenUserTaskMapId == 0)
            {
                return new JsonResult(false);
            }

            try
            {
                //업무참여자 정보
                GardenUserTaskMap gardenUserTaskMap = await _context.GardenUserTaskMap
                                                               .Include(gUserTaskMap => gUserTaskMap.GardenTask)
                                                               .FirstOrDefaultAsync(gUserTaskMap => gUserTaskMap.Id == gardenUserTaskMapId);
                if (gardenUserTaskMap == null)
                {
                    return new JsonResult(false);
                }

                List<GardenWorkTime> gardenWorkTime_list = new List<GardenWorkTime>();
                //업무참여자의 이달의 업무시간 목록
                if(startMonth != 0 && endMonth != 0)
                {
                    gardenWorkTime_list = await _context.GardenWorkTime
                                                      .AsNoTracking()
                                                      .Where(gardenWorkTime => gardenWorkTime.GardenUserId == gardenUserTaskMap.GardenUserId
                                                                          && gardenWorkTime.GardenTaskId == gardenUserTaskMap.GardenTaskId
                                                                          && gardenWorkTime.TaskDate.Month >= startMonth 
                                                                          && gardenWorkTime.TaskDate.Month <= endMonth)
                                                      .OrderBy(gardenWorkTime => gardenWorkTime.TaskDate)
                                                      .ToListAsync();
                }
                else
                {
                    int currentMonth = DateTime.Now.Month;
                    gardenWorkTime_list = await _context.GardenWorkTime
                                                        .AsNoTracking()
                                                        .Where(gardenWorkTime => gardenWorkTime.GardenUserId == gardenUserTaskMap.GardenUserId
                                                                            && gardenWorkTime.GardenTaskId == gardenUserTaskMap.GardenTaskId
                                                                            && gardenWorkTime.TaskDate.Month == currentMonth)
                                                        .OrderBy(gardenWorkTime => gardenWorkTime.TaskDate)
                                                        .ToListAsync();
                }
               




                if (gardenWorkTime_list.Count() == 0)                    
                    return new JsonResult("empty");

                List<object> object_list = new List<object>();
                foreach (GardenWorkTime gardenWorkTime in gardenWorkTime_list)
                {
                    object_list.Add(new
                    {
                        id = gardenWorkTime.Id,
                        taskDate = gardenWorkTime.TaskDate.ToShortDateString(),
                        startTime = gardenWorkTime.StartTime.Hours + ":" + gardenWorkTime.StartTime.Minutes,
                        endTime = gardenWorkTime.EndTime.Hours + ":" + gardenWorkTime.EndTime.Minutes,
                        isComplete = gardenWorkTime.IsComplete,
                        taskWeek = gardenWorkTime.TaskWeek
                    });
                }
                return new JsonResult(object_list);
            }
            catch (Exception ex)
            {
                string value = Convert.ToString(ex);
                return new JsonResult(false);
            }          
        }

        /// <summary>
        /// 정원 업무 참여자 목록 불러오기
        /// </summary>
        /// <param name="id">GardenTaskId</param>
        /// <returns></returns>
        public async Task<JsonResult> GetAttendUserList(int? id)
        {
            
            List<object> object_list = new List<object>();
            
            if(id == null)
            {
                var empty_jsonValue = new { data = object_list };
                return Json(empty_jsonValue);
            }

            List<GardenUserTaskMap> gardenUserTaskMap_list = await _context.GardenUserTaskMap
                                                                           .Include(gUserTaskMap => gUserTaskMap.GardenUser)
                                                                               .ThenInclude(gUser => gUser.User)
                                                                           .Include(gUserTaskMap => gUserTaskMap.GardenManager)
                                                                               .ThenInclude(gUser => gUser.User)
                                                                           .AsNoTracking()
                                                                           .Where(gUserTaskMap => gUserTaskMap.GardenTaskId == id)
                                                                           .ToListAsync();
            StringBuilder temp_roleTypeName = new StringBuilder();
            if (_globalValueService.IsActiveMembership)
            {
                foreach (GardenUserTaskMap gardenUserTaskMap in gardenUserTaskMap_list)
                {
                    GardenUser temp_gardenUserInfo = new GardenUser();

                    if (gardenUserTaskMap.GardenManagerId == null)
                    {
                        temp_gardenUserInfo = gardenUserTaskMap.GardenUser;
                        temp_roleTypeName.Append("참여자");
                    }
                    else
                    {
                        temp_gardenUserInfo = gardenUserTaskMap.GardenManager;
                        temp_roleTypeName.Append("담당자");
                    }


                    //수정해야됨
                    object_list.Add(new
                    {
                        roleType = temp_roleTypeName.ToString(),
                        userName = temp_gardenUserInfo.User.UserName,
                        name = temp_gardenUserInfo.User.Name,
                        isRent = gardenUserTaskMap.IsRent,
                        regDate = gardenUserTaskMap.RegDate.ToShortDateString(),
                        id = gardenUserTaskMap.Id
                    });

                    temp_roleTypeName.Clear();
                }
            }
            else
            {
                foreach (GardenUserTaskMap gardenUserTaskMap in gardenUserTaskMap_list)
                {
                    GardenUser temp_gardenUserInfo = new GardenUser();

                    if (gardenUserTaskMap.GardenManagerId == null)
                    {
                        temp_gardenUserInfo = gardenUserTaskMap.GardenUser;
                        temp_roleTypeName.Append("참여자");
                    }
                    else
                    {
                        temp_gardenUserInfo = gardenUserTaskMap.GardenManager;
                        temp_roleTypeName.Append("담당자");
                    }


                    //수정해야됨
                    object_list.Add(new
                    {
                        roleType = temp_roleTypeName.ToString(),
                        userName = temp_gardenUserInfo.Name,
                        name = temp_gardenUserInfo.Name,
                        isRent = gardenUserTaskMap.IsRent,
                        regDate = gardenUserTaskMap.RegDate.ToShortDateString(),
                        id = gardenUserTaskMap.Id
                    });

                    temp_roleTypeName.Clear();
                }
            }

            var jsonValue = new { data = object_list };

            return Json(jsonValue);
        }

        // GET: GardenUserTaskMaps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenUserTaskMap.Include(g => g.GardenTask);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GardenUserTaskMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUserTaskMap = await _context.GardenUserTaskMap
                .Include(g => g.GardenTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenUserTaskMap == null)
            {
                return NotFound();
            }

            return View(gardenUserTaskMap);
        }

        // GET: GardenUserTaskMaps/Create
        public IActionResult Create()
        {
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id");
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id");
            return View();
        }

        // POST: GardenUserTaskMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GardenUserId,GardenTaskId")] GardenUserTaskMap gardenUserTaskMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gardenUserTaskMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenUserTaskMap.GardenTaskId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenUserTaskMap.GardenUserId);
            return View(gardenUserTaskMap);
        }

        // GET: GardenUserTaskMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUserTaskMap = await _context.GardenUserTaskMap.FindAsync(id);
            if (gardenUserTaskMap == null)
            {
                return NotFound();
            }
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenUserTaskMap.GardenTaskId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenUserTaskMap.GardenUserId);
            return View(gardenUserTaskMap);
        }

        // POST: GardenUserTaskMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GardenUserId,GardenTaskId")] GardenUserTaskMap gardenUserTaskMap)
        {
            if (id != gardenUserTaskMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenUserTaskMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenUserTaskMapExists(gardenUserTaskMap.Id))
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
            ViewData["GardenTaskId"] = new SelectList(_context.GardenTask, "Id", "Id", gardenUserTaskMap.GardenTaskId);
            ViewData["GardenUserId"] = new SelectList(_context.GardenUser, "Id", "Id", gardenUserTaskMap.GardenUserId);
            return View(gardenUserTaskMap);
        }

        /// <summary>
        /// 업무참여자 삭제하기
        /// </summary>
        /// <param name="gardenUserTaskMapId"></param>
        /// <returns></returns>
        public async Task<JsonResult> DeleteAttendUser(int gardenUserTaskMapId)
        {
            try
            {
                GardenUserTaskMap gardenUserTaskMap = await _context.GardenUserTaskMap.FindAsync(gardenUserTaskMapId);

                if (gardenUserTaskMap == null)
                    return new JsonResult(false);

                _context.GardenUserTaskMap.Remove(gardenUserTaskMap);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }
        // GET: GardenUserTaskMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenUserTaskMap = await _context.GardenUserTaskMap
                .Include(g => g.GardenTask)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenUserTaskMap == null)
            {
                return NotFound();
            }

            return View(gardenUserTaskMap);
        }

        // POST: GardenUserTaskMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenUserTaskMap = await _context.GardenUserTaskMap.FindAsync(id);
            _context.GardenUserTaskMap.Remove(gardenUserTaskMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenUserTaskMapExists(int id)
        {
            return _context.GardenUserTaskMap.Any(e => e.Id == id);
        }
    }
}
