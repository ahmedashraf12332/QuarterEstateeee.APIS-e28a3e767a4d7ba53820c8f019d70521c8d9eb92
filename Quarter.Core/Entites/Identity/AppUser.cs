using Microsoft.AspNetCore.Identity;
using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
        public ICollection<UserFavoriteEstate> FavoriteEstates { get; set; }
        public Agent? Agent { get; set; }
    }
}
