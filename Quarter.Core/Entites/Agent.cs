using Quarter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entites
{
    public class Agent : BaseEntity<int>
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; } // رابط صورة الوكيل
        public string Description { get; set; }
        // علاقة عكسية: كل وكيل لديه مجموعة من العقارات
        public ICollection<Estate> Estates { get; set; }
        public string? AppUserId { get; set; }

        // Navigation Property: للوصول لبيانات المستخدم من الوكيل
        [ForeignKey(nameof(AppUserId))]
        public AppUser? AppUser { get; set; }
    }
}
