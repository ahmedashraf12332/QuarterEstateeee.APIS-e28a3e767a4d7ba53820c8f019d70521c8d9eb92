using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dto
{
    public class EstateTypeDto
    {
       
        public string Name { get; set; }  // زي 'Apartment', 'Villa', 'Commercial Building'
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;  // تاريخ الإنشاء
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;  // تاريخ التحديث
    }
}
