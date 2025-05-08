using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entites
{
    public class EstateLocation : BaseEntity<int>
    {
        public string City { get; set; }  // اسم المدينة

        public string Area { get; set; }  // اسم المنطقة
                                          //  public string Street { get; set; } 


    }
}
