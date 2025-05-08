using Quarter.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.ServiceContract
{
    public interface IProductService
    {
        Task<IEnumerable<EstateDto>> GetAllEstatesAsync();
        Task<IEnumerable<EstateLocationDto>> GetAllloctionAsync();
        Task<IEnumerable<EstateTypeDto>> GetAllTypeAsync();
        Task<EstateDto> GetEstateById(int id);
    }
}
