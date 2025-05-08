using Microsoft.AspNetCore.Identity;
using Quarter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            var user = new AppUser
            {
                Email = "ashraftiger83@gmail.com",
                DisplayName = "Ashraf Tiger",
                UserName = "ashraf",
                Address = new Address()
                {
                    FName = "Ashraf",
                    LName = "Tiger",
                    Street = "Street 1",
                    City = "Cairo",
                    Country = "Egypt"
                }
            };

            // Check if the user already exists, and if not, create it
            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                await _userManager.CreateAsync(user, "Ahmed123!"); // Replace with a secure password
            }
        }
    }
}
