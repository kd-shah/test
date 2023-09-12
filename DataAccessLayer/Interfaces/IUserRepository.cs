using Microsoft.AspNetCore.Identity;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using RealTimeChatApi.DataAccessLayer.Models;
using System.Security.Claims;

namespace RealTimeChatApi.DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(AppUser newUser, RegisterRequestDto UserObj);

        Task<AppUser> CheckExistingEmail(string email);

        Task<SignInResult> Authenticate(AppUser newUser, LoginRequestDto UserObj);

        Task<AppUser>CheckEmail(LoginRequestDto UserObj);

        Task<List<AppUser>> GetAllUsers(string currentUserId);
        
        Task<AppUser> GetCurrentUser();

        Task<IdentityResult> RegisterUserGoogle(AppUser newUser, string password);

    }
}
