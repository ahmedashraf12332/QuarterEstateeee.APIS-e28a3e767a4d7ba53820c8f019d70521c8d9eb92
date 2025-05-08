using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entites
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;  // تاريخ الإنشاء
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;  // تاريخ التحديث
    }
}
