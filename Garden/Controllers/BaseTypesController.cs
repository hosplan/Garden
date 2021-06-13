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
using System.Security.Claims;
using Garden.Helper;

namespace Garden.Controllers
{
    public class BaseTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGardenHelper _gardenHelper;
        public BaseTypesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _gardenHelper = gardenHelper;
        }

        // GET: BaseTypes
        public async Task<IActionResult> Index()
        {
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            return View(await _context.BaseType.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// 베이스 타입에 속한 베이스 서브타입 목록을 가져온다.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBaseSubTypeList(string id)
        {
            if (string.IsNullOrEmpty(id))
                id = _context.BaseType.First().Id;

            ViewData["BaseTypeId"] = id;

            List<BaseSubType> baseSubType_list = _context.BaseSubType
                                                         .AsNoTracking()
                                                         .Where(z => z.BaseTypeId == id)
                                                         .ToList();

            List<object> returnValue_object_list = new List<object>();
            
            foreach(BaseSubType baseSubType in baseSubType_list)
            {
                returnValue_object_list.Add(new
                {        
                    type = baseSubType.BaseTypeId,
                    name = baseSubType.Name,
                    description = baseSubType.Description,
                    id = baseSubType.Id
                });
            }

            var jsonResult = new { data = returnValue_object_list };
            return Json(jsonResult);
        }

        // GET: BaseTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseType = await _context.BaseType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseType == null)
            {
                return NotFound();
            }

            return View(baseType);
        }

        // GET: BaseTypes/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: BaseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsSubTypeEditable")] BaseType baseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baseType);
                await _context.SaveChangesAsync();
            }
            return PartialView();
        }

        // GET: BaseTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseType = await _context.BaseType.FindAsync(id);
            if (baseType == null)
            {
                return NotFound();
            }
            return PartialView(baseType);
        }

        // POST: BaseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,IsSubTypeEditable")] BaseType baseType)
        {
            if (id != baseType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseTypeExists(baseType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return RedirectToAction("Index");
        }

        // GET: BaseTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseType = await _context.BaseType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseType == null)
            {
                return NotFound();
            }

            return PartialView(baseType);
        }

        // POST: BaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baseType = await _context.BaseType.FindAsync(id);
            _context.BaseType.Remove(baseType);
            await _context.SaveChangesAsync();
            return PartialView(baseType);
        }

        private bool BaseTypeExists(string id)
        {
            return _context.BaseType.Any(e => e.Id == id);
        }
    }
}
