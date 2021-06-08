using Microsoft.EntityFrameworkCore;
using SBDA.API.DBContext;
using SBDA.API.IServices;
using SBDA.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.Repository
{
    public class BloodGroupRepository : IBloodGroupService
    {
        private readonly AppDbContext _context;
        public BloodGroupRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<BloodGroup> GetBloodGroupByIdAsync(int bloodGroupId)
        {
            return await _context.BloodGroups.FirstOrDefaultAsync(c => c.BloodGroupID == bloodGroupId);
        }

        public async Task<IEnumerable<BloodGroup>> GetAllBloodGroupsAsync()
        {
            return await _context.BloodGroups.ToListAsync();
        }


        public async Task<BaseAPIResponse> AddBloodGroupAsync(BloodGroup model)
        {
            var result = await _context.BloodGroups.AddAsync(model);
            await _context.SaveChangesAsync();
            if (result != null)
            {
                return new BaseAPIResponse
                {
                    IsSuccess = true,
                    Message = "Blood group add successfully"
                };

            }
            return new BaseAPIResponse
            {
                IsSuccess = false,
                Message = "Blood group add failed"
            };
        }

        public async Task<BaseAPIResponse> UpdateBloodGroupAsync(BloodGroup model)
        {
            var result = await _context.BloodGroups
                .FirstOrDefaultAsync(e => e.BloodGroupID == model.BloodGroupID);
            if (result != null)
            {
                result.BloodGroupName = model.BloodGroupName;
                await _context.SaveChangesAsync();
                return new BaseAPIResponse
                {
                    IsSuccess = true,
                    Message = "Blood group update successfully"
                };
            }
            return new BaseAPIResponse
            {
                IsSuccess = false,
                Message = "Blood group update failed"
            };
        }

        public async Task<BaseAPIResponse> DeleteBloodGroupByIdAsync(int bloodGroupId)
        {
            var result = await _context.BloodGroups
               .FirstOrDefaultAsync(e => e.BloodGroupID == bloodGroupId);
            if (result != null)
            {
                _context.BloodGroups.Remove(result);
                await _context.SaveChangesAsync();
                return new BaseAPIResponse { 
                    IsSuccess=true,
                    Message="Blood group delete successfully"
                };
            }
            return new BaseAPIResponse
            {
                IsSuccess = false,
                Message = "Blood group delete failed!"
            };
        }


    }
}
