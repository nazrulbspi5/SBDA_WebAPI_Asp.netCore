using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SBDA.API.AuthModels;
using SBDA.API.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IMailService _mailService;
        private IConfiguration _configuration;
        public AuthController(IUserService userService, IMailService mailService,IConfiguration configuration)
        {
            _userService = userService;
            _mailService = mailService;
            _configuration = configuration;
        }
        //api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);//Status code 200
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest("Some properties are not valid");//Status code 400
        }
        //api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    //await _mailService.SendEmailAsync("nazrulbspi5@gmail.com", "New Login", "<h1>new login to your account</h1><p>New login to your account at " + DateTime.Now + "</p>");
                    return Ok(result);//Status code 200
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
            }
            return BadRequest("Some properties are not valid");//Status code 400
        }
        //api/auth/confiremail?userId&token
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }
            var result =await _userService.ConfirmEmailAsync(userId, token);
            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
            }
            return BadRequest(result);
        }
        //api/auth/forgotpassword
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return NotFound();
            }
            var result =await _userService.ForgotPasswordAsync(email);
            if (result.IsSuccess)
            {
                return Ok(result);//200
            }
            return BadRequest(result);//400
        }
        //api/auth/addrole
        [HttpPost("addrole")]
        public async Task<ActionResult> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return NotFound();
            }
            var result = await _userService.CreateRoleAsync(roleName);
            if (result.IsSuccess)
            {
                return Ok(result);//200
            }
            return BadRequest(result);//400
        }
        //api/auth/allrole
        [HttpGet("allrole")]
        public  IEnumerable<IdentityRole> GetAllRole()
        {
            var identityRole = _userService.AllRolesAsync();
            return identityRole;
        }
    }
}
