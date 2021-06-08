using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SBDA.API.AuthModels;
using SBDA.API.DBContext;
using SBDA.API.IServices;
using SBDA.Models.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SBDA.API.Repository
{
    public class UserRepository : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly AppDbContext _context;
        public UserRepository(UserManager<IdentityUser> userManager, IConfiguration configuration, IMailService mailService,RoleManager<IdentityRole> roleManager, AppDbContext context )
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._mailService = mailService;
            this._context = context;
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManager.ConfirmEmailAsync(user, normalToken);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Email confirm successfully."
                };
            }
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Email did not confirmed",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var role = _userManager.GetRolesAsync(user);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that user name",
                    IsSuccess = false,
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            var member = _context.Members.FirstOrDefault(c => c.UserId == user.Id);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password!",
                    IsSuccess = false,
                };
            }
            var claims = new[]
            {
               new Claim("UserName",user.UserName),
               new Claim("Name",member.Name),
               new Claim(ClaimTypes.NameIdentifier,user.Id),
               new Claim(ClaimTypes.Role,role.ToString()),
             

        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
            var tokens = new JwtSecurityToken(
               issuer: _configuration["AuthSettings:Issuer"],
               audience: _configuration["AuthSettings:Audience"],
               claims: claims,
               expires: DateTime.Now.AddDays(30),
               signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
               );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(tokens);
            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = tokens.ValidTo,
                UserInfo = claims.ToDictionary(c => c.Type, c => c.Value)
            };

        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            //model.RoleName = "Admin";
            if (model == null)
            {
                throw new NullReferenceException("Register model is null");
            }
            if (model.Password != model.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Confirm Password doesn't match the password",
                    IsSuccess = false
                };
            
            var identityUser = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.UserName,
            };
           
            var result = await _userManager.CreateAsync(identityUser, model.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser,model.RoleName);
                var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                var encodedEmailToken = Encoding.UTF8.GetBytes(emailConfirmToken);
                var validEmailtoken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                string url = $"{_configuration["AppUrl"]}/api/auth/confirmemail?userId={identityUser.Id}&token={validEmailtoken}";
                await _mailService.SendEmailAsync(identityUser.Email, "Confirm Your Email", $"<h1>Welcome to SBDA<h1>" +
                                         $"<p>Please confirm your email address <a href='{url}'>Clicking Here</a></p>");

                Member member = new Member();
                member.Name = model.Name;
                member.Mobile = model.Mobile;
                member.PhotoPath = "Photo Url";
                member.UserId = identityUser.Id;
                member.Email =model.Email;
                member.BloodGroupId = model.BloodGroupId;
                member.Address = model.Address;
                member.EntryDate = DateTime.Now;

                _context.Add(member);
                await _context.SaveChangesAsync();
                return new UserManagerResponse
                {
                    Message = "User create succesfully.",
                    IsSuccess = true,
                };
            }
            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };

        }

        public async Task<UserManagerResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "No user associated with email"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";
            await _mailService.SendEmailAsync(email, "Reset Password", "<h2>Follow the instruction to reset your password</h2>" +
                $"To reset your password<a href='{url}'>Clicking Here</a></p>");
            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = "Reset password URL has been sent to the email successfully."
            };
        }

        public async Task<UserManagerResponse> CreateRoleAsync(string roleName)
        {
            //var roleName = await _roleManager.CreateAsync(identityUser);
            //await _userManager.AddToRoleAsync(identityUser, "Admin");

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var roleExist = await _roleManager.FindByNameAsync(roleName);
                if (roleExist==null)
                {
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = roleName
                    };
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        return new UserManagerResponse
                        {
                            IsSuccess = true,
                            Message = "Role create has been successfully."
                        };
                    }
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Role create failed!"
                    };
                }
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = $"Role name '{roleName}' is already taken!"
                };

            }
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Some property does not bind"
            };
        }

        public IEnumerable<IdentityRole> AllRolesAsync()
        {
           return  _roleManager.Roles;
        }
    }
}
