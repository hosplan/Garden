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
    public class BaseSubTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGardenHelper _gardenHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseSubTypesController(ApplicationDbContext context, IGardenHelper gardenHelper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _gardenHelper = gardenHelper;
            _httpContextAccessor = httpContextAccessor;

        }

        // GET: BaseSubTypes
        public  IActionResult Index()
        {
            //check read permission
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                            this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                            this.ControllerContext.RouteData.Values["action"].ToString());
            if (!isRead)
                return RedirectToAction("NotAccess","Home");

            List<BaseSubType> baseSubType_list = _context.BaseSubType
                                                            .Include(baseSubType => baseSubType.BaseType)
                                                            .AsNoTracking()
                                                            .ToList();

            return View(baseSubType_list);
        }

        // GET: BaseSubTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseSubType = await _context.BaseSubType
                .Include(b => b.BaseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseSubType == null)
            {
                return NotFound();
            }

            return View(baseSubType);
        }

        /// <summary>
        /// 관련 서브타입 생성
        /// </summary>
        /// <param name="id">baseTypeId</param>
        /// <returns></returns>
        // GET: BaseSubTypes/Create
        public IActionResult Create()
        {
            ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Name");
            return PartialView();
        }

        // POST: BaseSubTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BaseTypeId,Name,Description")] BaseSubType baseSubType)
        {

            if (ModelState.IsValid)
            {
                _context.Add(baseSubType);
                await _context.SaveChangesAsync(); 
            }

            ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Name", baseSubType.BaseTypeId);
            return PartialView();
        }

        // GET: BaseSubTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseSubType = await _context.BaseSubType.FindAsync(id);
            if (baseSubType == null)
            {
                return NotFound();
            }
            //ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Id", baseSubType.BaseTypeId);
            return PartialView(baseSubType);
        }

        // POST: BaseSubTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,BaseTypeId,Name,Description")] BaseSubType baseSubType)
        {
            if (id != baseSubType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseSubType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!string.IsNullOrEmpty(baseSubType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            //ViewData["BaseTypeId"] = new SelectList(_context.Set<BaseType>(), "Id", "Id", baseSubType.BaseTypeId);
            return RedirectToAction("Index", "BaseTypes");
            //return PartialView(baseSubType);
        }

        // GET: BaseSubTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baseSubType = await _context.BaseSubType
                .Include(b => b.BaseType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseSubType == null)
            {
                return NotFound();
            }

            return PartialView(baseSubType);
        }

        // POST: BaseSubTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baseSubType = await _context.BaseSubType.FindAsync(id);
            _context.BaseSubType.Remove(baseSubType);
            await _context.SaveChangesAsync();

            return PartialView(baseSubType);
        }

        private bool BaseSubTypeExists(string id)
        {
            return _context.BaseSubType.Any(e => e.Id == id);
        }
    }
}
