using SBDA.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.IServices
{
    public interface IBloodGroupService
    {
        Task<IEnumerable<BloodGroup>> GetAllBloodGroupsAsync();
        Task<BloodGroup> GetBloodGroupByIdAsync(int bloodGroupId);
        Task<BaseAPIResponse> AddBloodGroupAsync(BloodGroup model);
        Task<BaseAPIResponse> UpdateBloodGroupAsync(BloodGroup model);
        Task<BaseAPIResponse> DeleteBloodGroupByIdAsync(int bloodGroupId);
    }
}
