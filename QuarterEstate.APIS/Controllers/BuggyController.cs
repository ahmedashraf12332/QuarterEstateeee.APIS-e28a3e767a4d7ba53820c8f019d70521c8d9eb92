using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quarter.Repostory.Data.Context;
using QuarterEstate.APIS.Errors;
using System.Threading.Tasks;

namespace Quarter.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly QuarterDbContexts _context;

        public BuggyController(QuarterDbContexts context)
        {
            _context = context;
        }

        [HttpGet("NotFound")]
        public async Task<IActionResult> GEtNotFoundRequestError()
        {
            var brand = await _context.EstateLocations.FindAsync(100);
            if (brand is null) return NotFound(new ApiErrorResponse(404));
            return Ok(brand);
        }

        [HttpGet("ServerErorr")]
        public async Task<IActionResult> GEtServerError()
        {
            var brand = await _context.EstateLocations.FindAsync(100);
            var brandtoString = brand.ToString();
            return Ok(brand);
        }

        [HttpGet("badrequest")]
        public async Task<IActionResult> GEtBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public async Task<IActionResult> GEtBadRequestError(int id)
        {
            return Ok();
        }

        [HttpGet("unaithorized")]
        public async Task<IActionResult> GEtUnauthorizedError(int id)
        {
            return Unauthorized(new ApiErrorResponse(414));
        }
    }
}
