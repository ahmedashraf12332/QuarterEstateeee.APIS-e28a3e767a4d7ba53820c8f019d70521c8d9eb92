using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Quarter.Core.Dto.Auth;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Services.Contract;
using Quarter.Repostory.Identity.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Service.Service.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly StoreIdentityDbContext _context;


        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService, StoreIdentityDbContext context)
        {
          _userManager = userManager;
            _signInManager = signInManager;
        _tokenService = tokenService;
            _context = context;
        }

      

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            
                return null;
         var result =await  _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            if (!result.Succeeded) return null;
            return new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),

            };
          
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if ( await CheckEmailExitsAsync(registerDto.Email)) return null;
            var user =new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0],
               
               PhoneNumber = registerDto.PhoneNumber
            };
           var result= await  _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded) return null;
            return new UserDto() { DisplayName = user.DisplayName, Email = user.Email, Token = await _tokenService.CreateTokenAsync(user, _userManager) };
        }
        public async Task<bool> CheckEmailExitsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        public async Task<IEnumerable<ManageUserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new ManageUserDto
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                }).ToListAsync();
        }

        public async Task<ManageUserDto> GetUserByIdAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return null;

            return new ManageUserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<ManageUserDto> AddUserAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email.Split("@")[0],
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return null;

            return new ManageUserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<bool> UpdateUserAsync(string id, UpdateUserDto dto)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            user.DisplayName = dto.DisplayName ?? user.DisplayName;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, dto.Email);
                if (!setEmailResult.Succeeded) return false;

                var setUserNameResult = await _userManager.SetUserNameAsync(user, dto.Email.Split('@')[0]);
                if (!setUserNameResult.Succeeded) return false;
            }

            return (await _userManager.UpdateAsync(user)).Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        public async Task<(List<UserDto> users, int totalCount)> GetAllUsersAsync(int pageIndex, int pageSize)
        {
            var totalCount = await _context.Users.CountAsync();

            var users = await _context.Users
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                })
                .ToListAsync();

            return (users, totalCount);
        }
    }
}
