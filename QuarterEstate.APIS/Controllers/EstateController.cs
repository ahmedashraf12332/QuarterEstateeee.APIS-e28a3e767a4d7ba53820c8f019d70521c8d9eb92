using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.Core.Dto;
using Quarter.Core.Entites;
using Quarter.Core.Helper;
using Quarter.Core.ServiceContract;
using Quarter.Core.Specifications.Estatee;
using Quarter.Repostory.Data.Context;
using Quarter.Service.Service;
using QuarterEstate.APIS.Errors;


namespace Quarter.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsController : ControllerBase
    {
        private readonly QuarterDbContexts _context;
        private readonly IProductService _EstateService;
        private readonly IBlobService _blobService;

       public ProductsController(IProductService EstateService, IBlobService blobService, QuarterDbContexts context)
{
    _EstateService = EstateService;
    _blobService = blobService;
    _context = context;
}

        [ProducesResponseType(typeof(PaginationResponse<EstateDto>), StatusCodes.Status200OK)]
        [HttpGet]
       
        public async Task<ActionResult<PaginationResponse<EstateDto>>> GetAllEstate([FromQuery] EstateSpecParams EstateSpec)
        {
            // Adjusted method call to match the signature of IProductService
            var result = await _EstateService.GetAllEstatesAsync();
            return Ok(result);
        }

        [HttpGet("EstateLocation")]
        [ProducesResponseType(typeof(IEnumerable<EstateLocationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EstateLocationDto>>> GetAllloction()
        {
            var result = await _EstateService.GetAllloctionAsync();
            return Ok(result);
        }

        [HttpGet("EstateType")]
        [ProducesResponseType(typeof(IEnumerable<EstateTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EstateTypeDto>>> GetAllTypes()
        {
            var result = await _EstateService.GetAllTypeAsync(); // Fixed service reference
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EstateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));
            var result = await _EstateService.GetEstateById(id.Value); // Fixed service reference
            if (result is null) return NotFound(new ApiErrorResponse(404));
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddEstate([FromForm] EstateDto dto, List<IFormFile> images)
        {
            var imageUrls = new List<string>();

            foreach (var image in images)
            {
                var url = await _blobService.UploadFileAsync(image, "estates");
                imageUrls.Add(url);
            }

            var estate = new Estate
            {
                Name = dto.Name,
                EstateTypeId = dto.EstateTypeId,
              
                EstateLocationId = dto.EstateLocationId,
                
                Price = dto.Price,
                SquareMeters = dto.SquareMeters,
                Description = dto.Description,
                Images = string.Join(",", imageUrls),
                NumOfBedrooms = dto.NumOfBedrooms,
                NumOfBathrooms = dto.NumOfBathrooms,
                NumOfFloor = dto.NumOfFloor
            };

            await _context.Estates.AddAsync(estate);
            await _context.SaveChangesAsync();

            return Ok(estate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstate(int id, [FromBody] EstateDto estateDto)
        {
            var updated = await _EstateService.UpdateEstateAsync(id, estateDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstate(int id)
        {
            var deleted = await _EstateService.DeleteEstateAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        

    }
}

