using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Specifications.Estatee
{
    public class EstatetWithCountSpecification : BaseSpecifications<Estate, int>
    {
        public EstatetWithCountSpecification(EstateSpecParams productSpec) : base(
           P =>
           (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
       && (!productSpec.BrandId.HasValue || productSpec.BrandId == P.EstateLocationId)
       && (!productSpec.TypeId.HasValue || productSpec.TypeId == P.EstateTypeId))
        {

        }
    }
}
