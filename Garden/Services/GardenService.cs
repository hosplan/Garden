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
        object GetResource(int id);
        object GetResource(int id, bool isActivate);
        IEnumerable<object> GetResouces();
        IEnumerable<object> GetResouces(bool isActivate);
        Task<bool> CreateResource(object resource);
        Task<bool> CreateResources(IEnumerable<object> resource);
        Task<bool> UpdateResource(object resource);
        Task<bool> UpdateResources(IEnumerable<object> resources);
        Task<bool> RemoveResource(object resource);
        Task<bool> RemoveResources(IEnumerable<object> resources);
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

        public object GetResource(int id)
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
        public object GetResource(int id, bool isActivate)
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
        public IEnumerable<object> GetResouces()
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
        public IEnumerable<object> GetResouces(bool isActivate)
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

        public async Task<bool> CreateResource(object resource)
        {
            try
            {
                _context.Add(ConvertGardenSpace(resource));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }



        public async Task<bool> CreateResources(IEnumerable<object> resources)
        {
            try
            {
                _context.AddRange(ConvertGardenSpaces(resources));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateResource(object resource)
        {
            try
            {
                _context.Update(ConvertGardenSpace(resource));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateResources(IEnumerable<object> resources)
        {
            try
            {
                _context.UpdateRange(ConvertGardenSpaces(resources));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveResource(object resource)
        {
            try
            {
                _context.Remove(ConvertGardenSpace(resource));
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveResources(IEnumerable<object> resources)
        {
            try
            {
                _context.RemoveRange(ConvertGardenSpaces(resources));
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
