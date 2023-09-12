using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using RealTimeChatApi.DataAccessLayer.Data;
using RealTimeChatApi.DataAccessLayer.Interfaces;
using RealTimeChatApi.DataAccessLayer.Models;
using System.Security.Claims;

namespace RealTimeChatApi.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public readonly RealTimeChatDbContext _authContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(RealTimeChatDbContext chatDbContext, 
            IHttpContextAccessor httpContextAccessor,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _authContext = chatDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> RegisterUserAsync(AppUser newUser, RegisterRequestDto UserObj)
        {
            return await _userManager.CreateAsync(newUser, UserObj.password);
         }

        public async Task<AppUser> CheckExistingEmail(string email)
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            return userExists;
        }


        public async Task<SignInResult> Authenticate(AppUser user, LoginRequestDto UserObj)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, UserObj.password, lockoutOnFailure: false);
        }

        public async Task<AppUser> CheckEmail(LoginRequestDto UserObj)
        {
            var user = await _userManager.FindByEmailAsync(UserObj.email);
            return user;
        }

        public async Task<List<AppUser>> GetAllUsers(string currentUserId)
        {
            var userList = await _authContext.Users
            .Where(u => u.Id != currentUserId)
            .ToListAsync();

            return userList;
        }



        public async Task<AppUser> GetCurrentUser()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId != null)
            {
               
                var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);

                return user;
            }

            return null;
        }


        public async Task<IdentityResult> RegisterUserGoogle(AppUser newUser, string password)
        {
            return await _userManager.CreateAsync(newUser, password);
        }
    }
}
