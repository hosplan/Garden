using Garden.Data;
using Garden.Helper;
using Garden.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Garden.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGardenHelper _gardenHelper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IGardenHelper gardenHelper)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _gardenHelper = gardenHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string loginedUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(loginedUserId))
            {
                return Redirect("/Identity/Account/Login");
            }
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetControllerAndActionName(string pathName)
        {
            try
            {
                string[] temp = pathName.Split('/');
                if (temp.Length < 3)
                {
                    temp = (pathName + "/Index").Split('/');
                }

                var permission_list = await _context.Permission
                                                .Include(z => z.Role)
                                                .Where(z => z.ControllerName == temp[1] &&
                                                            z.ActionName == temp[2])                                                   
                                                .Select( z => new
                                                {
                                                    name = _context.Roles.AsNoTracking().FirstOrDefault(r => r.Id == z.RoleId).Name,
                                                    isRead = z.IsRead,
                                                    isCreate = z.IsCreate,
                                                    isUpdate = z.IsUpdate,
                                                    isDelete = z.IsDelete
                                                })
                                                .AsNoTracking()
                                                .ToListAsync();

                var value = new { controller = temp[1], action = temp[2], permission = permission_list };

                return new JsonResult(value);
            }
            catch
            {
                return new JsonResult(false);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotAccess()
        {
            return View();
        }

        /// <summary>
        /// 이메일 중복체크
        /// </summary>
        /// <param name="register_email">등록자의 아이디</param>
        /// <returns></returns>
        public async Task<JsonResult> CheckEmail(string register_id)
        {
            if(string.IsNullOrEmpty(register_id))            
                return new JsonResult(false);
            
            IdentityUser user = await _context.Users.FirstOrDefaultAsync(z => z.UserName == register_id);

            if (user != null)
                return new JsonResult(false);

            return new JsonResult(true);
        }

        //사용자 관리 페이지 이동
        public IActionResult IndexForUser()
        {
            bool isRead = _gardenHelper.CheckReadPermission(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                                                           this.ControllerContext.RouteData.Values["controller"].ToString(),
                                                           this.ControllerContext.RouteData.Values["action"].ToString());

            if (!isRead)
                return RedirectToAction("NotAccess", "Home");

            List<ApplicationUser> userInfo_list = _context.Users.Where(z => z.NormalizedUserName != "SYSTEM" 
                                                                      && z.UserName != "SYSTEM")
                                                             .ToList();

            return View(userInfo_list);
        }
    }
}
