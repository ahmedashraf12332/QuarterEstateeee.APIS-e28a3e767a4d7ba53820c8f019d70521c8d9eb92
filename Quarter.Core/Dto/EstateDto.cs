using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dto
{
  public class EstateDto
    {
        public int Id { get; set; }
        public int EstateTypeId { get; set; }  // نوع العقار (شقة، فيلا، إلخ)
        public string?EstateTypeName { get; set; } // العلاقة مع النوع

        public int EstateLocationId { get; set; }  // موقع)
        public string? EstateLocationName { get; set; }  // العلاقة مع الموقع
        public string Name { get; set; }
        public int Price { get; set; }  // السعر
        public double SquareMeters { get; set; }  // المساحة بالمتر
        public string? Description { get; set; }  // وصف العقار
        public string Images { get; set; }  // الصور
        public int NumOfBedrooms { get; set; }  // عدد الغرف
        public int NumOfBathrooms { get; set; } // عدد الحمامات
        public int NumOfFloor { get; set; } // عدد طوابق
                                            //خواص بتاعتهم موجوده في كونفرج بتاعتهم في روبوستي متنساش
       
    }
}
