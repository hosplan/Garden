using Garden.Data;
using Garden.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garden.Controllers
{
    public class UserAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserAdminsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private void SavePermission(Permission permission, string action, bool isCheck, bool isUpdate)
        {
            //권한의 종류
            if (action == "create" && isCheck)
                permission.IsCreate = true;
            else if (action == "read" && isCheck)
                permission.IsRead = true;
            else if (action == "update" && isCheck)
                permission.IsUpdate = true;
            else if (action == "delete" && isCheck)
                permission.IsDelete = true;
            else if (action == "create" && !isCheck)
                permission.IsCreate = false;
            else if (action == "read" && !isCheck)
                permission.IsRead = false;
            else if (action == "update" && !isCheck)
                permission.IsUpdate = false;
            else if (action == "delete" && !isCheck)
                permission.IsDelete = false;


            //생성 또는 수정여부
            if (isUpdate)
                _context.Update(permission);
            else
                _context.Add(permission);


            _context.SaveChanges();
        }

        /// <summary>
        /// 사용자 활성화
        /// </summary>
        /// <param name="id">사용자 아아디</param>
        /// <param name="isCheck">체크 여부</param>
        /// <returns></returns>
        public async Task<JsonResult> ChangeActive(string id, bool isCheck)
        {
            try
            {
                ApplicationUser userInfo = _context.Users.FirstOrDefault(z => z.Id == id);

                if (userInfo == null)
                    return new JsonResult(false);

                userInfo.IsActive = isCheck == true ? true : false;

                _context.Update(userInfo);
                await _context.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch
            {
                return new JsonResult(false);
            }
        }

        /// <summary>
        /// 권한 변경
        /// </summary>
        /// <param name="id">역할 아이디</param>
        /// <param name="action">권한 종류</param>
        /// <param name="isCheck">체크 여부</param>
        /// <param name="controllerPath">컨트롤러 이름</param>
        /// <param name="actionPath">뷰 이름</param>
        /// <returns></returns>
        public JsonResult ChangePermission(string id, string action, bool isCheck, string controllerPath, string actionPath)
        {
            try
            {
                Permission permission = _context.Permission.FirstOrDefault(
                                                                z => z.RoleId == id && 
                                                                z.ControllerName == controllerPath &&
                                                                z.ActionName == actionPath);

                if(permission == null)
                {                    
                    Permission create_permission = new Permission();

                    create_permission.Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    create_permission.ControllerName = controllerPath;
                    create_permission.ActionName = actionPath;
                    create_permission.RoleId = id;

                    SavePermission(create_permission, action, isCheck, false);
                }
                else
                {
                    SavePermission(permission, action, isCheck, true);
                }
            }
            catch
            {
                return new JsonResult(false);
            }
            return new JsonResult(true);
        }
        /// <summary>
        /// 관리자를 제외한 전체 유저목록을 가져온다.
        /// </summary>
        /// <returns>json</returns>
        public JsonResult GetUserList()
        {
            List<object> object_list = new List<object>();

            List<ApplicationUser> user_list = _context.Users
                                                      .AsNoTracking()
                                                      .Where(z => z.NormalizedUserName != "SYSTEM" && z.UserName != "SYSTEM")
                                                      .ToList();

            StringBuilder roleName_list = new StringBuilder();

            foreach (ApplicationUser user in user_list)
            {
                var role_list = _context.UserRoles.AsNoTracking().Where(z => z.UserId == user.Id);

                foreach (var role in role_list)
                {
                    ApplicationRole roleInfo = _context.Roles.AsNoTracking().FirstOrDefault(z => z.Id == role.RoleId);
                    roleName_list.Append("<span class='badge badge-primary ml-1 mr-1 shadow'>" + roleInfo.Name + "</span>");
                }

                object_list.Add(new
                {                   
                   // Role = roleName_list,
                    userName = user.UserName,
                    name = user.Name,
                    userRole = roleName_list.Length == 0 ? "없음" : roleName_list.ToString(),
                    gardenInfo = _context.GardenUser.Where(z => z.UserId == user.Id).Count(),
                    isActive = user.IsActive,
                    id = user.Id,
                });
                roleName_list.Clear();
            }


            var jsonValue = new { data = object_list };

            return Json(jsonValue);
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 역할을 변경한다.
        /// </summary>
        /// <param name="userId">사용자 id</param>
        /// <param name="roleId">역할 id</param>
        /// <returns></returns>
        public async Task<JsonResult> RoleChange(string userId, string roleId, bool isCheck)
        {
            try
            {
                ApplicationUser user = await _context.Users.FirstOrDefaultAsync(z => z.Id == userId);
                ApplicationRole role = await _context.Roles.FirstOrDefaultAsync(z => z.Id == roleId);

                if(isCheck == true)
                    _userManager.AddToRoleAsync(user, role.Name).Wait();
                else
                    _userManager.RemoveFromRoleAsync(user, role.Name).Wait();


                return new JsonResult(true);
            }
            catch(Exception ex)
            {
                return new JsonResult(false);
            }
           
        }

        /// <summary>
        /// 사용자 역할 변경
        /// </summary>
        /// <param name="id">사용자 id</param>
        /// <returns></returns>
        public IActionResult Edit(string id)
        {
            List<ApplicationRole> role_list = _context.Roles.ToList();

            var user_role_list = _context.UserRoles.Where(z => z.UserId == id);

            Dictionary<string, string> role_isActive_dic = new Dictionary<string, string>();
            foreach(var user_role in user_role_list)
            {
                role_isActive_dic.Add(user_role.RoleId, "checked");
            }
            ViewData["role_isActive_dic"] = role_isActive_dic;
            ViewData["userId"] = id;

            return PartialView(role_list);
        }
    }
}
