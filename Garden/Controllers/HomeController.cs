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
