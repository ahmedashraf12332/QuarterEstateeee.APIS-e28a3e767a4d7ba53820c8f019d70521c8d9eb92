using Quarter.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Specifications.Estatee
{
    public class EstateSpecifications : BaseSpecifications<Estate, int>
    {
        public EstateSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public EstateSpecifications(EstateSpecParams EstateSpec) : base(
            P =>
            (string.IsNullOrEmpty(EstateSpec.Search) || P.Name.ToLower().Contains(EstateSpec.Search))
       && (!EstateSpec.EstateLocationId.HasValue || EstateSpec.EstateLocationId == P.EstateLocationId)
        && (!EstateSpec.EstateTypeId.HasValue || EstateSpec.EstateTypeId == P.EstateTypeId))





        {


            if (!string.IsNullOrEmpty(EstateSpec.Sort))
            {
                switch (EstateSpec.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(P => P.Name);
                        break;

                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
            ApplyIncludes();
            ApplyPagination(EstateSpec.PageSize, EstateSpec.PageSize * (EstateSpec.PageIndex - 1));
        }


        private void ApplyIncludes()
        {
            Includes.Add(P => P.EstateLocation);
            Includes.Add(P => P.EstateType);
        }
    }
}
