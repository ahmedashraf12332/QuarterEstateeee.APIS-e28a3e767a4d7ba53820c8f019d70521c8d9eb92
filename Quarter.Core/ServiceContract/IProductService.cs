using Quarter.Core.Dto;
using Quarter.Core.Helper;
using Quarter.Core.Specifications.Estatee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.ServiceContract
{
    public interface IProductService
    {
        Task<PaginationResponse<EstateDto>> GetAllEstatesAsync(EstateSpecParams estateSpec);
        Task<IEnumerable<EstateLocationDto>> GetAllloctionAsync();
        Task<IEnumerable<EstateTypeDto>> GetAllTypeAsync();
        Task<EstateDto> GetEstateById(int id);
        Task<EstateDto> CreateEstateAsync(EstateDto estateDto);
        Task<bool> UpdateEstateAsync(int id, EstateDto estateDto);
        Task<bool> DeleteEstateAsync(int id);
    }
}
