using Microsoft.AspNetCore.Identity;
using SBDA.API.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.IServices
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
        Task<UserManagerResponse> ForgotPasswordAsync(string email);
        Task<UserManagerResponse> CreateRoleAsync(string roleName);
        IEnumerable<IdentityRole> AllRolesAsync();
    }
}
