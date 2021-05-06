using Garden.Data;
using Garden.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Index()
        {

            return View();
        }
    }
}
