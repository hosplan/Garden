using Garden.Data;
using Garden.Models;
using Microsoft.AspNetCore.Http;
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
                var role_list = _context.Roles.ToList();
                
                foreach(var role in role_list)
                {
                    role_Dic.Add(role.Id, role.Name);
                }

                return role_Dic;
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
        public string loginUserName
        {
            get
            {
                return _context.Users.FirstOrDefault(user => user.Id == loginUserId).UserName;
            }
        }
    }
}
