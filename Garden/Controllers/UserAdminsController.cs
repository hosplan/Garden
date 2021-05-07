using Garden.Data;
using Garden.Models;
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
        
        public UserAdminsController(ApplicationDbContext context)
        {
            _context = context;
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
                    roleName_list.Append("<span class='badge badge-primary'>" + roleInfo.Name + "</span>");
                }

                object_list.Add(new
                {
                    Id = user.Id,
                    Role = roleName_list,
                    Name = user.UserName,
                    GardenInfo = _context.GardenUser.Where(z => z.UserId == user.Id).Count(),
                    IsActive = user.IsActive
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
    }
}
