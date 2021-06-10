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
    public interface IGardenHelper
    {
        int CreateGardenRole(int gardenSpace_id, string baseSubType_id);
        bool CreateGardenUser(int gardenSpace_id, string user_id, int? gardenRole_id);
        bool CheckReadPermission(string loginUserId, string controllerName, string actionName);
    }
    public class GardenHelper : IGardenHelper
    {
        private readonly ApplicationDbContext _context;

        public GardenHelper(ApplicationDbContext context)
        {
            _context = context;
        }
        #region 읽기 권한 조회
        public bool CheckReadPermission(string loginUserId, string controllerName, string actionName)
        {
            bool isRead = false;

            try
            {

                List<string> myRole_list = _context.UserRoles.Where(r => r.UserId == loginUserId).Select(z => z.RoleId).ToList();

                if (myRole_list == null || myRole_list.Count() == 0)
                    return isRead;

                List<Permission> permission_list = _context.Permission
                                                        .Include(z => z.Role)
                                                        .AsNoTracking()
                                                        .Where(z => z.ControllerName == controllerName
                                                                && z.ActionName == actionName
                                                                && z.IsRead == true
                                                                && myRole_list.Contains(z.RoleId))
                                                        .ToList();

                if (permission_list.Count() > 0)
                {
                    isRead = true;
                }
                else
                {
                    //관리자일경우 모든 권한이 다 있어야된다.
                    foreach (string myRole in myRole_list)
                    {
                        var role = _context.Roles.AsNoTracking().FirstOrDefault(z => z.Id == myRole);

                        if (role.Name == "Admin" && role.NormalizedName == "ADMIN")
                        {
                            Permission create_permission = new Permission();
                            create_permission.Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                            create_permission.RoleId = role.Id;
                            create_permission.ControllerName = controllerName;
                            create_permission.ActionName = actionName;
                            create_permission.IsCreate = true;
                            create_permission.IsRead = true;
                            create_permission.IsUpdate = true;
                            create_permission.IsDelete = true;

                            _context.Add(create_permission);
                            _context.SaveChanges();

                            isRead = true;
                            break;                            
                        }
                        
                    }
                }
                return isRead;
            }
            catch
            {
                return isRead;
            } 
        }
        #endregion

        #region 정원 유저 역할 초기 생성
        public int CreateGardenRole(int gardenSpace_id, string baseSubType_id)
        {
            int gardenRole_id = 0;
            try
            {
                GardenRole gardenRole = new GardenRole();

                gardenRole.GardenId = gardenSpace_id;
                gardenRole.SubTypeId = baseSubType_id;

                _context.Add(gardenRole);
                _context.SaveChanges();

                gardenRole_id = gardenRole.Id;
            }
            catch
            {
                
            }
            return gardenRole_id;
        }
        #endregion

        #region 정원 유저 생성
        public bool CreateGardenUser(int gardenSpace_id, string user_id, int? gardenRole_id)
        {
            try
            {
                GardenUser gardenUser = new GardenUser();

                gardenUser.UserId = user_id;
                gardenUser.IsActivate = true;
                gardenUser.GardenSpaceId = gardenSpace_id;
                gardenUser.CreateDate = DateTime.Now;

                if (gardenRole_id.HasValue)
                    gardenUser.GardenRoleId = gardenRole_id;

                _context.Add(gardenUser);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

      
    }
}
