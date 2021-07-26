using Garden.Data;
using Garden.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Services
{
    public interface IGardenService
    {
        object GetGardenSpace(int id);
        object GetGardenSpaceForActivate(int id, bool isActivate);
        IEnumerable<object> GetGardenSpaces();
        IEnumerable<object> GetGardenSpacesForActivate(bool isActivate);
        Task<bool> CreateGardenSpace(object gardenSpace);
        Task<bool> CreateGardenSpaces(IEnumerable<object> gardenSpaces);
        Task<bool> UpdateGardenSpace(object gardenSpace);
        Task<bool> UpdateGardenSpaces(IEnumerable<object> gardenSpaces);
        Task<bool> RemoveGardenSpace(object gardenSpace);
        Task<bool> RemoveGardenSpaces(IEnumerable<object> gardenSpaces);

        Task<bool> CreateGardenFee(object gardenFee);
        Task<bool> CreateGardenFees(IEnumerable<object> gardenFees);
        Task<List<object>> GetGardenUsers(int gardenSpaceId);
    }

    public class GardenService : IGardenService
    {
        private readonly ApplicationDbContext _context;
        public GardenService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        #region GardenSpaceService        
        //GardenSpace 로 변환
        private GardenSpace ConvertGardenSpace(object value)
        {
            return (GardenSpace)value;
        }

        //IEnumerable<GardenSpace> 로 변환
        private IEnumerable<GardenSpace> ConvertGardenSpaces(IEnumerable<object> values)
        {
            return (IEnumerable<GardenSpace>)values;
        }

        public object GetGardenSpace(int id)
        {
            object value = new object();
            try
            {
                GardenSpace gardenSpace = _context.GardenSpace.Find(id);

                if (gardenSpace == null)
                    return value;

                value = new
                {
                    id = gardenSpace.Id,
                    name = gardenSpace.Name,
                    createDate = gardenSpace.CreatedDate,
                    subTypeId = gardenSpace.SubTypeId,
                    isActivate = gardenSpace.IsActivate
                };

                return value;
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        /// 활성화된 GardenSpace 가져오기
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActivate">GardenSpace의 활성화 여부</param>
        /// <returns></returns>
        public object GetGardenSpaceForActivate(int id, bool isActivate)
        {
            object value = new object();
            try
            {
                GardenSpace gardenSpace = _context.GardenSpace
                                                    .AsNoTracking()
                                                    .FirstOrDefault(gardenSpace =>
                                                                    gardenSpace.Id == id &&
                                                                    gardenSpace.IsActivate == isActivate);

                if (gardenSpace == null)
                    return value;

                value = new
                {
                    id = gardenSpace.Id,
                    name = gardenSpace.Name,
                    createDate = gardenSpace.CreatedDate,
                    subTypeId = gardenSpace.SubTypeId,
                    isActivate = gardenSpace.IsActivate
                };

                return value;
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        /// GardenSpaces 전부 가져오기
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetGardenSpaces()
        {
            List<object> values = new List<object>();
            try
            {
                IEnumerable<GardenSpace> gardenSpaces = _context.GardenSpace.AsNoTracking().AsEnumerable();


                if (gardenSpaces.Count() == 0)
                    return values;

                foreach (GardenSpace gardenSpace in gardenSpaces)
                {
                    values.Add(new
                    {
                        id = gardenSpace.Id,
                        name = gardenSpace.Name,
                        createDate = gardenSpace.CreatedDate,
                        subTypeId = gardenSpace.SubTypeId,
                        isActivate = gardenSpace.IsActivate
                    });
                }

                return values;
            }
            catch
            {
                return values;
            }
        }

        /// <summary>
        /// 활성화된 GardenSpaces 전부 가져오기
        /// </summary>
        /// <param name="isActivate"></param>
        /// <returns></returns>
        public IEnumerable<object> GetGardenSpacesForActivate(bool isActivate)
        {
            List<object> values = new List<object>();
            try
            {
                IEnumerable<GardenSpace> gardenSpaces = _context.GardenSpace
                                                                .AsNoTracking()
                                                                .Where(gardenSpace => gardenSpace.IsActivate == isActivate)                                                               
                                                                .AsEnumerable();


                if (gardenSpaces.Count() == 0)
                    return values;

                foreach (GardenSpace gardenSpace in gardenSpaces)
                {
                    values.Add(new
                    {
                        id = gardenSpace.Id,
                        name = gardenSpace.Name,
                        createDate = gardenSpace.CreatedDate,
                        subTypeId = gardenSpace.SubTypeId,
                        isActivate = gardenSpace.IsActivate
                    });
                }

                return values;
            }
            catch
            {
                return values;
            }
        }

        public async Task<bool> CreateGardenSpace(object gardenSpace)
        {
            try
            {
                _context.Add(ConvertGardenSpace(gardenSpace));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }



        public async Task<bool> CreateGardenSpaces(IEnumerable<object> gardenSpaces)
        {
            try
            {
                _context.AddRange(ConvertGardenSpaces(gardenSpaces));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateGardenSpace(object gardenSpace)
        {
            try
            {
                _context.Update(ConvertGardenSpace(gardenSpace));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateGardenSpaces(IEnumerable<object> gardenSpaces)
        {
            try
            {
                _context.UpdateRange(ConvertGardenSpaces(gardenSpaces));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveGardenSpace(object gardenSpace)
        {
            try
            {
                _context.Remove(ConvertGardenSpace(gardenSpace));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveGardenSpaces(IEnumerable<object> gardenSpaces)
        {
            try
            {
                _context.RemoveRange(ConvertGardenSpaces(gardenSpaces));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region GardenUserService
        public async Task<List<object>> GetGardenUsers(int gardenSpaceId)
        {
            List<object> objects = new List<object>();

            try
            {
                List<GardenUser> gardenUsers = await _context.GardenUser
                                                             .AsNoTracking()
                                                             .Where(gardenUser => gardenUser.GardenSpaceId == gardenSpaceId)
                                                             .ToListAsync();
                
                foreach(GardenUser gardenUser in gardenUsers)
                {
                    objects.Add(new
                    {
                        id = gardenUser.Id,
                        gardenSpaceId = gardenUser.GardenSpaceId,
                        name = gardenUser.Name
                    });
                }
                return objects;
            }
            catch
            {
                return objects;
            }
        }
        #endregion

        #region GardenFee
        private GardenFee ConvertGardenFee(object value)
        {
            return (GardenFee)value;
        }

        private IEnumerable<GardenFee> ConvertGardenFees(IEnumerable<object> values)
        {
            return (IEnumerable<GardenFee>)values;
        }

        //CreateGardenFee
        //CreateGardenFees

        public async Task<bool> CreateGardenFee(object gardenFee)
        {
            try
            {
                _context.Add(ConvertGardenFee(gardenFee));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateGardenFees(IEnumerable<object> gardenFees)
        {
            try
            {
                _context.AddRange(ConvertGardenFees(gardenFees));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
