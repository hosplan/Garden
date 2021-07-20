using Garden.Data;
using Garden.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garden.Services
{
    public interface IGardenFeesService
    {
        GardenFee GetGardenFee(int id);
        IEnumerable<GardenFee> GetGardenFees(int id);
        Task CreateGardenFee(GardenFee gardenFee);
        Task CreateGardenFees(IEnumerable<GardenFee> gardenFees);
        Task UpdateGardenFee(GardenFee gardenFee);
        Task UpdateGardenFees(IEnumerable<GardenFee> gardenFees);
        Task RemoveGardenFee(GardenFee gardenFee);
        Task RemoveGardenFees(IEnumerable<GardenFee> gardenFees);
    }
    public class GardenFeesService : IGardenFeesService
    {
        private readonly ApplicationDbContext _context;
        public GardenFeesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public GardenFee GetGardenFee(int id)
        {
            try
            {
                return _context.GardenFee.Find(id);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<GardenFee> GetGardenFees(int id)
        {
            try
            {
                return _context.GardenFee.Where(gardenFee => gardenFee.Id == id).AsEnumerable();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task CreateGardenFee(GardenFee gardenFee)
        {
            try
            {
                _context.Add(gardenFee);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task CreateGardenFees(IEnumerable<GardenFee> gardenFees)
        {
            try
            {
                _context.AddRange(gardenFees);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task UpdateGardenFee(GardenFee gardenFee)
        {
            try
            {
                _context.Update(gardenFee);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task UpdateGardenFees(IEnumerable<GardenFee> gardenFees)
        {
            try
            {
                _context.UpdateRange(gardenFees);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task RemoveGardenFee(GardenFee gardenFee)
        {
            try
            {
                _context.Remove(gardenFee);
                await _context.SaveChangesAsync();              
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task RemoveGardenFees(IEnumerable<GardenFee> gardenFees)
        {
            try
            {
                _context.RemoveRange(gardenFees);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
