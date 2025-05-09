using AutoMapper;
using Quarter.Core;
using Quarter.Core.Dto;
using Quarter.Core.Entites;
using Quarter.Core.ServiceContract;
using Quarter.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Quarter.Core.ServiceContract;
using Quarter.Core.Helper;
using Quarter.Core.Specifications.Estatee;
namespace Quarter.Service.Service.Estates
{
    public class EstateService : IProductService
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public EstateService(IUnitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EstateLocationDto>> GetAllloctionAsync()
        {
            return _mapper.Map<IEnumerable<EstateLocationDto>>(await _unitOfWork.Repository<EstateLocation, int>().GetAllAsync());
        }

       

        public async Task<IEnumerable<EstateTypeDto>> GetAllTypeAsync()
        {
            return _mapper.Map<IEnumerable<EstateTypeDto>>(await _unitOfWork.Repository<EstateType, int>().GetAllAsync());
        }

        public async Task<EstateDto> GetEstateById(int id)
        {
            var spec = new EstateSpecifications(id);
            return _mapper.Map<EstateDto>(await _unitOfWork.Repository<Estate, int>().GetWithSpecAsync(spec));
        }

       
       

    
      
        public async Task<EstateDto> CreateEstateAsync(EstateDto estateDto)
        {
            var estate = new Estate
            {
                EstateTypeId = estateDto.EstateTypeId,
                EstateLocationId = estateDto.EstateLocationId,
                Name = estateDto.Name,
                Price = estateDto.Price,
                SquareMeters = estateDto.SquareMeters,
                Description = estateDto.Description,
                Images = estateDto.Images,
                NumOfBedrooms = estateDto.NumOfBedrooms,
                NumOfBathrooms = estateDto.NumOfBathrooms,
                NumOfFloor = estateDto.NumOfFloor
            };

            await _unitOfWork.Repository<Estate, int>().AddAsync(estate);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<EstateDto>(estate);
        }

        public async Task<bool> UpdateEstateAsync(int id, EstateDto estateDto)
        {
            var existing = await _unitOfWork.Repository<Estate, int>().GetAsync(id);
            if (existing == null) return false;

            _mapper.Map(estateDto, existing);
            _unitOfWork.Repository<Estate, int>().Update(existing);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteEstateAsync(int id)
        {
            var existing = await _unitOfWork.Repository<Estate, int>().GetAsync(id);
            if (existing == null) return false;

            _unitOfWork.Repository<Estate, int>().Delete(existing);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<PaginationResponse<EstateDto>> GetAllEstatesAsync(EstateSpecParams estateSpec)
        {
            var spec = new EstateSpecifications(estateSpec);
            var products = await _unitOfWork.Repository<Estate, int>().GetAllWithSpecAsync(spec);
            var mappedProducts = _mapper.Map<IEnumerable<EstateDto>>(products);
            var countSpec = new EstatetWithCountSpecification(estateSpec);
            var count = await _unitOfWork.Repository<Estate, int>().GetCountAsync(countSpec);
            return new PaginationResponse<EstateDto>(estateSpec.PageSize, estateSpec.PageIndex, count, mappedProducts);

        }
    }
}
