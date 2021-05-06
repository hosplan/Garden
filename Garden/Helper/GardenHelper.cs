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
    public interface IGardenHelper
    {
        int CreateGardenRole(int gardenSpace_id, string baseSubType_id);

        bool CreateGardenUser(int gardenSpace_id, string user_id, int? gardenRole_id);
    }
    public class GardenHelper : IGardenHelper
    {
        private readonly ApplicationDbContext _context;

        public GardenHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        
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
