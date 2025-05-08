using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quarter.Core.Dto;
using Quarter.Core.Helper;
using Quarter.Core.ServiceContract;
using Quarter.Core.Specifications.Estatee;
using QuarterEstate.APIS.Errors;


namespace Quarter.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _EstateService;

        public ProductsController(IProductService EstateService)
        {
            _EstateService = EstateService;
        }
        [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme)]
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
        [ProducesResponseType(typeof( EstateDto ), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));
            var result = await _EstateService.GetEstateById(id.Value); // Fixed service reference
            if (result is null) return NotFound(new ApiErrorResponse(404));
            return Ok(result);
        }
    }
}
