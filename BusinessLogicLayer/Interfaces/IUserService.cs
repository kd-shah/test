using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using RealTimeChatApi.BusinessLogicLayer.Services;
using RealTimeChatApi.DataAccessLayer.Models;

namespace RealTimeChatApi.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegisterUserAsync(RegisterRequestDto UserObj);

        Task<IActionResult> Authenticate(LoginRequestDto UserObj);

        Task<IActionResult> GetAllUsers();

        //Task<IActionResult> GoogleLogin();

        //Task<IActionResult> GoogleResponse();

        Task<string> AuthenticateGoogle(ExternalAuthRequestDto request);

        Task<AppUser> AuthenticateGoogleUserAsync(ExternalAuthRequestDto request);
    }
}
