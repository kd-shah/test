using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RealTimeChatApi.DataAccessLayer.Models;
using RealTimeChatApi.BusinessLogicLayer.Interfaces;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace RealTimeChatApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpGet("google-login")]
        //public async Task <IActionResult> GoogleLogin()
        //{
        //    return await _userService.GoogleLogin();
        //}

        //[HttpGet("google-response")]
        //[Authorize(AuthenticationSchemes = "Google")]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    return await _userService.GoogleResponse();
        //}

        [HttpPost("GoogleAuthenticate")]
        public async Task<string> GoogleAuthenticate([FromBody] ExternalAuthRequestDto request)
        {
            return await _userService.AuthenticateGoogle(request);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterRequestDto UserObj)
        {

            return await _userService.RegisterUserAsync(UserObj);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto UserObj)
        {
            return await _userService.Authenticate(UserObj);
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {

            return await _userService.GetAllUsers();
        }

    }
}
