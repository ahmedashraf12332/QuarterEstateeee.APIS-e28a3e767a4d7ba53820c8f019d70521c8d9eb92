using Quarter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entites
{
    public class UserFavoriteEstate
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EstateId { get; set; }
        public DateTime AddedDate { get; set; }

        // Navigation Properties (اختياري بس مهم)
        public AppUser User { get; set; }
        public Estate Estate { get; set; }
    }

}
