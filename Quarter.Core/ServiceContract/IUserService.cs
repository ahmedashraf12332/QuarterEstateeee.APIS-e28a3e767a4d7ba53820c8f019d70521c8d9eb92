using Microsoft.AspNetCore.Identity;
using Quarter.Core.Dto.Auth;
using Quarter.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Services.Contract
{
    public interface IUserService
    {
    Task<UserDto>    LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> CheckEmailExitsAsync(string email);
        Task<IEnumerable<ManageUserDto>> GetAllUsersAsync();
        Task<ManageUserDto> GetUserByIdAsync(string id);
        Task<ManageUserDto> AddUserAsync(RegisterDto dto);
        Task<bool> UpdateUserAsync(string id, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(string id);
        Task<(List<UserDto> users, int totalCount)> GetAllUsersAsync(int pageIndex, int pageSize);
        Task<bool> ChangeUserRoleAsync(string userId, string newRole);
        Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName);
        Task<bool> LinkUserToAgentAsync(string userId, int agentId);
        Task<UserDto?> GetUserForAgentAsync(int agentId);
    }
}

