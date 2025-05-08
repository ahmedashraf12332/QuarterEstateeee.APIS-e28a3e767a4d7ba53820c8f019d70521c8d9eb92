using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entites
{
    public class EstateType : BaseEntity<int>
    {
        public string Name { get; set; }  // زي 'Apartment', 'Villa', 'Commercial Building'

    }
}
