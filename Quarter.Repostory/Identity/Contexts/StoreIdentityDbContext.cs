using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quarter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repostory.Identity.Contexts
{
    public class StoreIdentityDbContext : IdentityDbContext<AppUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
        {

        }
    }
}
