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
    public class GardenWorkTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GardenWorkTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GardenWorkTimes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GardenWorkTime.Include(g => g.GardenSpace);
            return View(await applicationDbContext.ToListAsync());
        }

        /// <summary>
        /// 완료처리
        /// </summary>
        /// <param name="id">gardenWorkTimeId</param>
        /// <returns></returns>
        public async Task<IActionResult> CompleteForWorkItem(int id, int spaceId)
        {
            GardenWorkTime gardenWorkTime = _context.GardenWorkTime.Find(id);

            if(gardenWorkTime == null)
                return RedirectToAction("Details", "GardenSpaces", new { id = spaceId });

            gardenWorkTime.IsComplete = gardenWorkTime.IsComplete == true ? false : true;

            try
            {
                _context.Update(gardenWorkTime);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return RedirectToAction("Details", "GardenSpaces", new { id = spaceId });
            }
           
            return RedirectToAction("Details", "GardenSpaces", new { id = spaceId });
        }

        // GET: GardenWorkTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenWorkTime = await _context.GardenWorkTime
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenWorkTime == null)
            {
                return NotFound();
            }

            return View(gardenWorkTime);
        }      

        /// <summary>
        /// 업무시간 업데이트
        /// </summary>
        /// <param name="gardenWorkTimeId"></param>
        /// <param name="taskDate"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateWorkTime(int gardenWorkTimeId, DateTime taskDate, TimeSpan startTime, TimeSpan endTime)
        {
            GardenWorkTime gardenWorkTime = _context.GardenWorkTime.Find(gardenWorkTimeId);

            gardenWorkTime.TaskDate = taskDate;
            gardenWorkTime.StartTime = startTime;
            gardenWorkTime.EndTime = endTime;

            _context.Update(gardenWorkTime);
            await _context.SaveChangesAsync();

            return new JsonResult(true);
        }


        public async Task<JsonResult> DeleteWorkTime(int gardenWorkTimeId)
        {
            GardenWorkTime gardenWorkTime = _context.GardenWorkTime.Find(gardenWorkTimeId);

            try
            {
                _context.Remove(gardenWorkTime);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }
          
            return new JsonResult(true);
        }

        public async Task<JsonResult> CompleteWorkTime(int gardenWorkTimeId, bool isComplete)
        {
            try
            {
                GardenWorkTime gardenWorkTime = await _context.GardenWorkTime.FindAsync(gardenWorkTimeId);

                if (gardenWorkTime == null)
                    return new JsonResult(false);

                gardenWorkTime.IsComplete = isComplete;
                _context.Update(gardenWorkTime);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }

        // GET: GardenWorkTimes/Create
        public IActionResult Create(int gardenUserTaskMapId, int gardenSpaceId)
        {
            GardenUserTaskMap gardenUserTaskMap = _context.GardenUserTaskMap
                                                          .Include(gUserTaskMap => gUserTaskMap.GardenUser)
                                                            .ThenInclude(gUser => gUser.User)
                                                          .AsNoTracking()
                                                          .FirstOrDefault(gUserTaskMap => gUserTaskMap.Id == gardenUserTaskMapId);

            if (gardenUserTaskMap == null)
                return NotFound();

            ViewData["GardenSpaceId"] = gardenSpaceId;
            ViewData["GardenTaskId"] = gardenUserTaskMap.GardenTaskId;
            ViewData["GardenUserId"] = gardenUserTaskMap.GardenUserId; 
            ViewData["GardenUserName"] = gardenUserTaskMap.GardenUser.User.Name;

            return View();
        }

        private int GetGardenWorkTimeStartDayofWeek(DateTime startTaskDate)
        {
            if(startTaskDate.DayOfWeek == DayOfWeek.Monday)
                return 1;
            else if(startTaskDate.DayOfWeek == DayOfWeek.Tuesday)
                return 2;
            else if(startTaskDate.DayOfWeek == DayOfWeek.Wednesday)
                return 3;
            else if(startTaskDate.DayOfWeek == DayOfWeek.Thursday)
                return 4;
            else if (startTaskDate.DayOfWeek == DayOfWeek.Friday)
                return 5;
            else if (startTaskDate.DayOfWeek == DayOfWeek.Saturday)
                return 6;
            else if (startTaskDate.DayOfWeek == DayOfWeek.Sunday)
                return 0;

            return 7;
        }

        /// <summary>
        /// (몇)주차에 따른 수강 날짜 생성
        /// </summary>
        /// <param name="startDayofWeek">시작요일</param>
        /// <param name="weekend">주중의 선택한 요일</param>
        /// <param name="taskDate">시작 날짜</param>
        /// <param name="taskWeek">(몇)주차</param>
        /// <returns></returns>
        private List<DateTime> MakeTaskDateList(int startDayofWeek, Weekend weekend, DateTime taskDate, int taskWeek)
        {

            List<DateTime> dateTime_list = new List<DateTime>();
            List<int> taskDate_dayOfWeek_list = new List<int>();

            //먼저 선택된 요일에 대한 값(int)를 넣어준다.
            if(weekend.IsMon == true)
                taskDate_dayOfWeek_list.Add(1);
            if(weekend.IsTue == true)
                taskDate_dayOfWeek_list.Add(2);
            if(weekend.IsWed == true)
                taskDate_dayOfWeek_list.Add(3);
            if(weekend.IsThr == true)
                taskDate_dayOfWeek_list.Add(4);
            if(weekend.IsFri == true)
                taskDate_dayOfWeek_list.Add(5);
            if(weekend.IsSat == true)
                taskDate_dayOfWeek_list.Add(6);
            if(weekend.IsSun == true)
                taskDate_dayOfWeek_list.Add(0);

            for (int i = 0; i < taskDate_dayOfWeek_list.Count(); i++)
            {
                int SubtractValue = 0;

                //시작날짜의 요일보다 앞의 요일을 골랐을때 (즉 다음주)
                // ex ) 시작요일 - 목요일 / 다음 선택 요일 - 수요일
                if (taskDate_dayOfWeek_list[i] < startDayofWeek)
                    SubtractValue = (7 - startDayofWeek) + taskDate_dayOfWeek_list[i];
                else if (taskDate_dayOfWeek_list[i] == startDayofWeek)
                    SubtractValue = 0;
                else
                    SubtractValue = taskDate_dayOfWeek_list[i] - startDayofWeek;

                DateTime tempDate = taskDate.AddDays(SubtractValue);

                for (int j = 0; j < taskWeek; j++)
                {
                    dateTime_list.Add(tempDate);
                    tempDate = tempDate.AddDays(7);
                }
            }
           
            return dateTime_list;
        }

        // POST: GardenWorkTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,GardenSpaceId,TaskDate,TaskWeek,weekend,GardenTaskId")] GardenWorkTime gardenWorkTime, int GardenUserId)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    //시작날짜의 요일값
                    int startDayofWeek = GetGardenWorkTimeStartDayofWeek(gardenWorkTime.TaskDate);

                    if (startDayofWeek > 6)
                        return NotFound();

                    //업무날짜 가져오기
                    List<DateTime> taskDateTime_list = MakeTaskDateList(startDayofWeek, gardenWorkTime.weekend, gardenWorkTime.TaskDate, gardenWorkTime.TaskWeek);

                    List<GardenWorkTime> gardenWorkTime_list = new List<GardenWorkTime>();

                    foreach (DateTime taskDateTime in taskDateTime_list)
                    {
                        gardenWorkTime_list.Add(new GardenWorkTime
                        {
                            StartTime = gardenWorkTime.StartTime,
                            EndTime = gardenWorkTime.EndTime,
                            GardenSpaceId = gardenWorkTime.GardenSpaceId,
                            GardenTaskId = gardenWorkTime.GardenTaskId,
                            TaskDate = taskDateTime,
                            TaskWeek = gardenWorkTime.TaskWeek,
                            GardenUserId = GardenUserId
                        });
                    }

                    _context.AddRange(gardenWorkTime_list);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    
                }
               
                return RedirectToAction("Details","GardenTasks", new { id = gardenWorkTime.GardenTaskId });
            }
            //ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenWorkTime.GardenSpaceId);
            return RedirectToAction("Details", "GardenTasks", new { id = gardenWorkTime.GardenTaskId });
        }

        // GET: GardenWorkTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenWorkTime = await _context.GardenWorkTime.FindAsync(id);
            if (gardenWorkTime == null)
            {
                return NotFound();
            }
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenWorkTime.GardenSpaceId);
            return View(gardenWorkTime);
        }

        // POST: GardenWorkTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,GardenSpaceId")] GardenWorkTime gardenWorkTime)
        {
            if (id != gardenWorkTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gardenWorkTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenWorkTimeExists(gardenWorkTime.Id))
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
            ViewData["GardenSpaceId"] = new SelectList(_context.GardenSpace, "Id", "Id", gardenWorkTime.GardenSpaceId);
            return View(gardenWorkTime);
        }

        // GET: GardenWorkTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gardenWorkTime = await _context.GardenWorkTime
                .Include(g => g.GardenSpace)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gardenWorkTime == null)
            {
                return NotFound();
            }

            return View(gardenWorkTime);
        }

        // POST: GardenWorkTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gardenWorkTime = await _context.GardenWorkTime.FindAsync(id);
            _context.GardenWorkTime.Remove(gardenWorkTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GardenWorkTimeExists(int id)
        {
            return _context.GardenWorkTime.Any(e => e.Id == id);
        }
    }
}
