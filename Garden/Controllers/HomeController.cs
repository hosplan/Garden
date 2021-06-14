using Garden.Data;
using Garden.Helper;
using Garden.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        //private void testMethod()
        //{
        //    try
        //    {
        //        string connection = "server = 192.168.0.128; uid=sa; pwd = emth022944w!; database = MPS;";

        //        SqlConnection sqlConn = new SqlConnection(connection);
        //        SqlCommand sql_cmd = new SqlCommand();
        //        sql_cmd.Connection = sqlConn;
        //        sql_cmd.CommandText = "select * from Teacher where IsActive = 1";
        //        sqlConn.Open();

        //        List<GardenUser> gardenUser_list = new List<GardenUser>();

        //        using (SqlDataReader read_data = sql_cmd.ExecuteReader())
        //        {
        //            while (read_data.Read())
        //            {
                        
        //                GardenUser gardenUser = new GardenUser();
        //                gardenUser.Name = read_data[7].ToString();
        //                gardenUser.Age = Convert.ToInt32(read_data[1].ToString());
        //                gardenUser.TempString = read_data[2].ToString();
        //                gardenUser.Tel = read_data[3].ToString();
        //                gardenUser.Address = read_data[4].ToString();
        //                gardenUser.Description = read_data[5].ToString();
        //                gardenUser.IsActiveDate = Convert.ToDateTime(read_data[6].ToString());
        //                gardenUser.BirthDay = Convert.ToDateTime(read_data[13].ToString());
        //                gardenUser.GardenRoleId = 3;
        //                gardenUser.GardenSpaceId = 1;
        //                gardenUser_list.Add(gardenUser);
                        
        //            }
        //        }
        //        _context.AddRange(gardenUser_list);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = Convert.ToString(ex);
        //    }
        //}

        [HttpGet]
        public IActionResult Index()
        {
            //testMethod();
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
        public IActionResult NoLicense()
        {
            return View();
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
