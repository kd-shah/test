using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatApi.DataAccessLayer.Models;
using RealTimeChatApi.BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Identity;
using RealTimeChatApi.BusinessLogicLayer.Interfaces;
using RealTimeChatApi.DataAccessLayer.Interfaces;
using static Google.Apis.Auth.GoogleJsonWebSignature;


namespace RealTimeChatApi.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
       
        
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        
        public UserService( IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
           
            _configuration = configuration;
            _userRepository = userRepository;
        }

        
        public async Task<string> AuthenticateGoogle([FromBody] ExternalAuthRequestDto request)
        {
            //if (!ModelState.IsValid)
            //    return new BadRequestObjectResult(ModelState.Values.SelectMany(it => it.Errors).Select(it => it.ErrorMessage));
            var token = CreateJwt(await AuthenticateGoogleUserAsync(request));
            return token;
        }

        public async Task<AppUser> AuthenticateGoogleUserAsync(ExternalAuthRequestDto request)
        {
            Payload payload = await ValidateAsync(request.IdToken, new ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            });

            return (AppUser)await GetOrCreateExternalLoginUser(ExternalAuthRequestDto.PROVIDER, payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);
        }


        private async Task<IdentityUser> GetOrCreateExternalLoginUser(string provider, string key, string email, string firstName, string lastName)
        {
            var user = await _userManager.FindByLoginAsync(provider, key);


            if (user != null)
                return user;

            user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // If the email is not found, try to create the user with the provided firstName as the username
                user = new AppUser
                {
                    Email = email,
                    UserName = firstName,
                    Id = key,
                };

            }
            var userName = await _userManager.FindByNameAsync(firstName);
            if (userName != null)
            {
                // If the email exists and the username (firstName) is also taken, generate a unique username
                string newUserName = firstName;
                int count = 1;
                while (userName != null)
                {
                    newUserName = $"{firstName}{count:D2}"; // Appending a unique number to the username
                    userName = await _userManager.FindByNameAsync(newUserName);
                    count++;
                }
                user.UserName = newUserName;
                await _userManager.UpdateAsync(user);
            }
            await _userManager.CreateAsync(user);
            var info = new UserLoginInfo(provider, key, provider.ToUpperInvariant());
            var result = await _userManager.AddLoginAsync(user, info);

            if (result.Succeeded)
                return user;

            return null;
        }

        // 





        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterRequestDto UserObj)
        {

            if (UserObj == null)
                return new BadRequestObjectResult(new { Message = "Invalid request" });

            if (!IsValidEmail(UserObj.email))
                return new BadRequestObjectResult(new { Message = "Invalid email format" });

            if (!IsValidPassword(UserObj.password))
                return new BadRequestObjectResult(new { Message = "Invalid password format" });

            // Check if the user already exists
            var existingUser = await _userRepository.CheckExistingEmail(UserObj.email);

            //var existingUser = await _userManager.FindByEmailAsync(UserObj.email);
            if (existingUser == null)
                return new ConflictObjectResult(new { message = "Registration failed because the email is already registered" });


            var newUser = new AppUser
            {
                Name = UserObj.name,
                UserName = UserObj.email,
                Email = UserObj.email,
                Token = ""

            };

            //var result = await _userManager.CreateAsync(newUser, UserObj.password);
            var result = await _userRepository.RegisterUserAsync(newUser, UserObj);

            if (result.Succeeded)
            {
                
                return new OkObjectResult(new { Message = "User Registered", newUser });
            }
            else
            {
                return new BadRequestObjectResult(new { Message = "User registration failed", Errors = result.Errors });
            }
        }


        public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto UserObj)
        {
            if (UserObj == null)
                return new BadRequestObjectResult(new { Message = "Invalid request" });

            if (!IsValidEmail(UserObj.email))
                return new BadRequestObjectResult(new { Message = "Invalid email format" });

            // Use UserManager to find the user by email
            var user = await _userRepository.CheckEmail(UserObj);
            //var user = await _userManager.FindByEmailAsync(UserObj.email);
            if (user == null)
                return new NotFoundObjectResult(new { Message = "Login failed due to incorrect credentials" });

            // Use SignInManager to check the user's password
            //var result = await _signInManager.CheckPasswordSignInAsync(user, UserObj.password, lockoutOnFailure: false);
            var result = await _userRepository.Authenticate(user, UserObj);

            if (result.Succeeded)
            {
                // Authentication succeeded, you can generate a token or return additional user information here.
                var response = new LoginResponseDto
                {
                    name = user.Name,
                    email = user.Email,
                    token = CreateJwt(user)
                };

                // Generate a token or perform any other post-authentication logic

                return new OkObjectResult(new
                {
                    Message = "Login Success",
                    UserInfo = response, 
                });
            }
            else
            {
                // Authentication failed
                return new BadRequestObjectResult(new
                {
                    Message = "Incorrect Password or Invalid Credentials"
                });
            }

        }

        public async Task<IActionResult> GetAllUsers()
        {
            var currentUser = await GetCurrentUser();

            if (currentUser.Id == null)
            {
                return new BadRequestObjectResult(new { Message = "Unable to retrieve current user." });
            }

            var userList = await _userRepository.GetAllUsers(currentUser.Id);


            return new OkObjectResult(new { users = userList });
        }

        public async Task<AppUser> GetCurrentUser()
        {
            return await _userRepository.GetCurrentUser();
        }

        private string CreateJwt(AppUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("It Is A Secret Key Which Should Not Be Shared With Other Users.....");

            var claims = new List<Claim>
    {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };
            var identity = new ClaimsIdentity(claims);


            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        private bool IsValidPassword(string password)
        {
            int requiredLength = 8;
            if (password.Length < requiredLength)
                return false;

            return true;
        }


    }
}
