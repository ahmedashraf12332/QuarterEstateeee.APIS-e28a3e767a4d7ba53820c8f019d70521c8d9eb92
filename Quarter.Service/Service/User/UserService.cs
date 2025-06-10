using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Quarter.Core;
using Quarter.Core.Dto.Auth;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Entites;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Services.Contract;
using Quarter.Repostory;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitofWork _unitOfWork;
        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService, StoreIdentityDbContext context,RoleManager<IdentityRole> roleManager ,  IUnitofWork unitOfWork)
        {
          _userManager = userManager;
            _signInManager = signInManager;
        _tokenService = tokenService;
            _context = context;
            _roleManager = roleManager;

            _unitOfWork = unitOfWork;
        }



        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return null;

            // --- بداية التعديل ---
            // 1. احصل على صلاحيات المستخدم
            var roles = await _userManager.GetRolesAsync(user);

            // 2. قم بإنشاء التوكن (سيحتوي على الصلاحيات تلقائيًا بفضل إعداداتنا)
            var token = await _tokenService.CreateTokenAsync(user, _userManager);
            // --- نهاية التعديل ---

            return new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Token = token,
                Roles = roles // <-- أرجع قائمة الصلاحيات في الاستجابة
            };
        }
          
        

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await CheckEmailExitsAsync(registerDto.Email)) return null;
            var user = new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0],
                PhoneNumber = registerDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return null;

            // === بداية التعديل: إضافة المستخدم إلى صلاحية "User" ===
            await _userManager.AddToRoleAsync(user, "User");
            // === نهاية التعديل ===

            return new UserDto()
            {
                Id = user.Id, // <== من الأفضل إرجاع الـ ID أيضاً
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
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
        public async Task<bool> ChangeUserRoleAsync(string userId, string newRole)
        {
            // 1. ابحث عن المستخدم المطلوب
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false; // المستخدم غير موجود

            // 2. تأكد أن الصلاحية الجديدة موجودة في النظام
            if (!await _roleManager.RoleExistsAsync(newRole)) return false; // الصلاحية غير موجودة

            // 3. احصل على صلاحيات المستخدم الحالية
            var currentRoles = await _userManager.GetRolesAsync(user);

            // 4. قم بإزالة المستخدم من كل صلاحياته الحالية
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // 5. أضف المستخدم إلى الصلاحية الجديدة
            var result = await _userManager.AddToRoleAsync(user, newRole);

            return result.Succeeded;
        }
        public async Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName)
        {
            // 1. استخدم UserManager لجلب كل المستخدمين الذين ينتمون لصلاحية معينة
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            // 2. قم بتحويل قائمة المستخدمين (من نوع AppUser) إلى قائمة من نوع UserDto
            //    لإرجاع البيانات المطلوبة فقط بشكل آمن
            var usersDto = usersInRole.Select(user => new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                // بما أننا نعرف الصلاحية، يمكننا إضافتها مباشرة
                Roles = new List<string> { roleName }
            });

            return usersDto;
        }
        // File: Quarter.Service.Service.User/UserService.cs

        public async Task<bool> LinkUserToAgentAsync(string userId, int agentId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Agent"))
            {
                return false;
            }

            // --- بداية التعديل ---
            // حدد نوع الكيان (Agent) ونوع المفتاح (int)
            var agentRepo = _unitOfWork.Repository<Agent, int>();
            // --- نهاية التعديل ---

            var agent = await agentRepo.GetAsync(agentId);
            if (agent == null)
            {
                return false;
            }

            agent.AppUserId = userId;

            // --- بداية التعديل ---
            agentRepo.Update(agent);
            // --- نهاية التعديل ---

            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }
        public async Task<UserDto?> GetUserForAgentAsync(int agentId)
        {
            // 1. ابحث عن الوكيل باستخدام الـ ID الخاص به
            var agentRepo = _unitOfWork.Repository<Agent, int>();
            var agent = await agentRepo.GetAsync(agentId); // نستخدم GetAsync حسب تعريفك

            if (agent == null)
            {
                // إذا لم يتم العثور على الوكيل، لا يوجد مستخدم مرتبط به
                return null;
            }

            // 2. تأكد من أن الوكيل مرتبط بالفعل بمستخدم
            if (string.IsNullOrEmpty(agent.AppUserId))
            {
                // الوكيل موجود ولكنه غير مرتبط بأي حساب مستخدم
                return null;
            }

            // 3. استخدم AppUserId الذي حصلنا عليه للبحث عن المستخدم في نظام Identity
            var user = await _userManager.FindByIdAsync(agent.AppUserId);

            if (user == null)
            {
                // هذا يعني أن هناك ID مستخدم مسجل ولكنه غير موجود، وهي حالة نادرة (قد تحدث إذا حُذف المستخدم)
                return null;
            }

            // 4. قم بتحويل بيانات المستخدم إلى UserDto لإرجاعها
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles
                // لا نرجع التوكن هنا لأنه خاص بعملية تسجيل الدخول فقط
            };
        }
    }
}
