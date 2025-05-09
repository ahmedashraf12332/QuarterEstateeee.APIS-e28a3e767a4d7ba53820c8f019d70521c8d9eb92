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
       && (!productSpec.EstateLocationId.HasValue || productSpec.EstateLocationId == P.EstateLocationId)
       && (!productSpec.EstateTypeId.HasValue || productSpec.EstateTypeId == P.EstateTypeId))
        {

        }
    }
}
