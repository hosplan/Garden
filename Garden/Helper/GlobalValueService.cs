using Garden.Data;
using Garden.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Garden.Helper
{
    public class GlobalValueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;

        public GlobalValueService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
        public Dictionary<string, string> GardenRoleName
        {
            get
            {
                Dictionary<string, string> role_Dic = new Dictionary<string, string>();
                var role_list = _context.Roles.ToList().OrderBy(z => z.Grade);
                
                foreach(var role in role_list)
                {
                    role_Dic.Add(role.Id, role.Name);
                }

                return role_Dic;
            }            
        }

        /// <summary>
        /// 역할의 등급값 가져오기(가장 높은값)
        /// </summary>
        public int GetGrade
        {
            get
            {

                var userRole_list = _context.UserRoles.Where(z => z.UserId == loginUserId);
                List<int> grade = new List<int>();
                foreach(var userRole in userRole_list)
                {
                    ApplicationRole roleInfo = _context.Roles.OrderBy(z => z.Grade).FirstOrDefault(z => z.Id == userRole.RoleId);
                    grade.Add(roleInfo.Grade);                        
                }

                if (grade.Count() == 0)
                    return -1;

                return grade.First();
            }
        }
        public Permission GetPermission
        {
            get
            {
                Permission returnPermission = new Permission();
                try
                {
                    var routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
                    if (routeValues.ContainsKey("controller") && routeValues.ContainsKey("action"))
                    {
                        List<string> roleId_list = _context.UserRoles.Where(r => r.UserId == loginUserId).Select(z => z.RoleId).ToList();
                        List<Permission> permission_list = _context.Permission.Where(z => z.ControllerName == (string)routeValues["controller"]
                                                                                        && z.ActionName == (string)routeValues["action"]
                                                                                        && roleId_list.Contains(z.RoleId)).ToList();

                        foreach (var permission in permission_list)
                        {
                            returnPermission.IsCreate = (returnPermission.IsCreate || permission.IsCreate);
                            returnPermission.IsRead = (returnPermission.IsRead || permission.IsRead);
                            returnPermission.IsUpdate = (returnPermission.IsUpdate || permission.IsUpdate);
                            returnPermission.IsDelete = (returnPermission.IsDelete || permission.IsDelete);
                        }
                    }
                    return returnPermission;
                }
                catch
                {
                    return new Permission();
                }


            }
        }
        public bool IsActiveSystem
        {
            get
            {
                try
                {
                    List<GardenSystem> gardenSystems = _context.GardenSystem.ToList();

                    if (gardenSystems.Count() == 0)
                        return false;

                    GardenSystem gardenSystem = _context.GardenSystem.First();

                    if (gardenSystem.IsActive == false)
                        return false;
                    else if (gardenSystem.License != "dmsalsxogh")
                        return false;
                    else
                        return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool IsActiveMembership
        {
            get
            {                
                return _context.GardenSystem.First().ActiveMembership;
            }
        }
        public string loginUserName
        {
            get
            {
                return _context.Users.FirstOrDefault(user => user.Id == loginUserId).UserName;
            }
        }

        //#region 읽기 권한 조회
        //public bool CheckReadPermission(string loginUserId, string controllerName, string actionName)
        //{
        //    bool isRead = false;

        //    try
        //    {
        //        List<string> myRole_list = _context.UserRoles.Where(r => r.UserId == loginUserId).Select(z => z.RoleId).ToList();

        //        if (myRole_list == null || myRole_list.Count() == 0)
        //            return isRead;

        //        List<Permission> permission_list = _context.Permission
        //                                                .Include(z => z.Role)
        //                                                .AsNoTracking()
        //                                                .Where(z => z.ControllerName == controllerName
        //                                                        && z.ActionName == actionName
        //                                                        && z.IsRead == true
        //                                                        && myRole_list.Contains(z.RoleId))
        //                                                .ToList();

        //        if (permission_list.Count() > 0)
        //            isRead = true;

        //    }
        //    catch
        //    {
        //        return isRead;
        //    }

        //    return isRead;
        //}
        //#endregion
    }
}
