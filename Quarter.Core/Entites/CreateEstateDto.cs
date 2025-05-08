using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Entites
{
    public class CreateEstateDto
    {
        public int EstateTypeId { get; set; }
        public int EstateLocationId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int SquareMeters { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public int NumOfBedrooms { get; set; }
        public int NumOfBathrooms { get; set; }
        public int NumOfFloor { get; set; }

    }
}
