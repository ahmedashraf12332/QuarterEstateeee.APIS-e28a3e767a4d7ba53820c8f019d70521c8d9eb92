using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dto
{
    public class EstateLocationDto
    {
        public int Id { get; set; }
        public string? City { get; set; }  // اسم المدينة

        public string Area { get; set; }  // اسم المنطقة
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;  // تاريخ الإنشاء
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;  // تاريخ التحديث//  public string Street { get; set; }
    }
}
