using Garden.Data;
using Garden.Models;
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
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
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
    }
}
