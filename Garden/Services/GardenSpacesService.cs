using Garden.Data;
using Garden.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Services
{
    public interface IGardenSpacesService
    {
        GardenSpace GetGardenSpace(int id);

        IEnumerable<GardenSpace> GetGardenSpaces(int id);
        //리소스 생성 - 단일
        bool CreateResource(GardenSpace resource);
        //리소스 생성 - 여러개
        bool CreateResources(IEnumerable<GardenSpace> resource);
        bool UpdateResource(GardenSpace resource);
        bool UpdateResources(IEnumerable<GardenSpace> resources);
        bool RemoveResource(GardenSpace resource);
        bool RemoveResources(IEnumerable<GardenSpace> resources);
    }

    public class GardenSpacesService : IGardenSpacesService
    {
        private readonly ApplicationDbContext _context;

        public GardenSpace GetGardenSpace(int id)
        {
            try
            {
                return _context.GardenSpace.Find(id);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<GardenSpace> GetGardenSpaces(int id)
        {
            try
            {
                return _context.GardenSpace.Where(gardenSpace => gardenSpace.Id == id).AsEnumerable();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public GardenSpacesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateResource(GardenSpace resource)
        {
            try
            {
                _context.Add(resource);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateResources(IEnumerable<GardenSpace> resources)
        {
            try
            {
                _context.AddRange(resources);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateResource(GardenSpace resource)
        {
            try
            {
                _context.Update(resource);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateResources(IEnumerable<GardenSpace> resources)
        {
            try
            {
                _context.UpdateRange(resources);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveResource(GardenSpace resource)
        {
            try
            {
                _context.Remove(resource);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveResources(IEnumerable<GardenSpace> resources)
        {
            try
            {
                _context.RemoveRange(resources);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
