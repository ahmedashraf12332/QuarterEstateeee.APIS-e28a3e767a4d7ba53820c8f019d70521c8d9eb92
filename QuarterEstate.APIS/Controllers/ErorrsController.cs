using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuarterEstate.APIS.Errors;


namespace Quarter.APIS.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErorrsController : ControllerBase
    {
        public IActionResult Erorr(int code)
        {
            return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));

        }
    }
}
