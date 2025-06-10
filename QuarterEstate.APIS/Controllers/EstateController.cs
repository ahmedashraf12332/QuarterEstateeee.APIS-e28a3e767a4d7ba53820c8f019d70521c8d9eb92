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
using Quarter.Service.Service.Estates;
using QuarterEstate.APIS.Errors;
using System.Security.Claims;


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
        public async Task<ActionResult<PaginationResponse<EstateDto>>> GetAllProduct([FromQuery] EstateSpecParams estateSpec)
        {
            // ملاحظة: يجب تعديل السيرفس الداخلية GetAllEstatesAsync لتشمل بيانات الوكيل ونوع المعاملة
            var result = await _EstateService.GetAllEstatesAsync(estateSpec);
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
            // ملاحظة: يجب تعديل السيرفس الداخلية GetEstateById لتشمل بيانات الوكيل ونوع المعاملة
            if (id is null) return BadRequest(new ApiErrorResponse(400));
            var result = await _EstateService.GetEstateById(id.Value);
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
                NumOfFloor = dto.NumOfFloor,
              TransactionType = (Quarter.Core.Entites.TransactionType)dto.TransactionType, // Casting
                AgentId = dto.AgentId.Value

            };

            await _context.Estates.AddAsync(estate);
            await _context.SaveChangesAsync();

            return Ok("Estate created successfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstate(int id, [FromBody] EstateDto estateDto)
        {
            var updated = await _EstateService.UpdateEstateAsync(id, estateDto);
            if (!updated) return NotFound();
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstate(int id)
        {
            var deleted = await _EstateService.DeleteEstateAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        [HttpPost("EstateLocation")]
        public async Task<IActionResult> AddEstateLocation([FromBody] EstateLocation estateLocation)
        {
            if (string.IsNullOrEmpty(estateLocation.Area))
                return BadRequest("Area is required");

            _context.EstateLocations.Add(estateLocation);
            await _context.SaveChangesAsync();
            return Ok(estateLocation);
        }

        // حذف Area عن طريق Id
        [HttpDelete("EstateLocation/{id}")]
        public async Task<IActionResult> DeleteEstateLocation(int id)
        {
            var entity = await _context.EstateLocations.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.EstateLocations.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("EstateType")]
        public async Task<IActionResult> AddEstateType([FromBody] EstateType estateType)
        {
            if (string.IsNullOrEmpty(estateType.Name))
                return BadRequest("Name is required");

            _context.EstateTypes.Add(estateType);
            await _context.SaveChangesAsync();
            return Ok(estateType);
        }

        // حذف Name عن طريق Id
        [HttpDelete("EstateType/{id}")]
        public async Task<IActionResult> DeleteEstateType(int id)
        {
            var entity = await _context.EstateTypes.FindAsync(id);
            if (entity == null)
                return NotFound();

            _context.EstateTypes.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // دالة مساعدة لجلب الـ UserId الحالي من الـ JWT Token
        // يفضل وضعها في Base Controller أو Helper Class لو هتستخدمها في أكتر من مكان
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// 1. جلب جميع العقارات المفضلة للمستخدم الحالي.
        /// GET: api/Products/favorites
        /// </summary>
        /// <returns>قائمة (List) من الـ Estate (أو EstateDto) للعقارات المفضلة.</returns>
        [HttpGet("favorites")] // ده الـ Route للـ Endpoint: api/Products/favorites
        [Authorize] // هذا الـ Endpoint يتطلب تسجيل دخول
        [ProducesResponseType(typeof(IEnumerable<EstateDto>), StatusCodes.Status200OK)] // عشان الـ Swagger
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<EstateDto>>> GetFavorites()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not identified."); // المستخدم مش مسجل دخول
            }

            // استرجاع العقارات المفضلة للمستخدم الحالي
            // Include الـ Estate عشان نجيب بيانات العقار
            // Select الـ Estate عشان نرجع العقارات نفسها مش الـ UserFavoriteEstate Object
            var favorites = await _context.UserFavoriteEstates
                .Where(f => f.UserId == userId)
                .Include(f => f.Estate)
                    .ThenInclude(e => e.EstateType) // تحميل نوع العقار
                .Include(f => f.Estate.EstateLocation) // تحميل موقع العقار
                .Select(f => new EstateDto // تحويل لـ EstateDto مباشرة
                {
                    Id = f.Estate.Id,
                    Name = f.Estate.Name,
                    Price = f.Estate.Price,
                    SquareMeters = f.Estate.SquareMeters,
                    Description = f.Estate.Description,
                    Images = f.Estate.Images,
                    NumOfBedrooms = f.Estate.NumOfBedrooms,
                    NumOfBathrooms = f.Estate.NumOfBathrooms,
                    NumOfFloor = f.Estate.NumOfFloor,
                    EstateTypeId = f.Estate.EstateTypeId,
                    EstateTypeName = f.Estate.EstateType != null ? f.Estate.EstateType.Name : null,
                    EstateLocationId = f.Estate.EstateLocationId,
                    EstateLocationName = f.Estate.EstateLocation != null ? f.Estate.EstateLocation.Area : null
                })
                .ToListAsync();

            return Ok(favorites);
        }

        /// <summary>
        /// 2. إضافة عقار للمفضلة للمستخدم الحالي.
        /// POST: api/Products/favorites/{estateId}
        /// </summary>
        /// <param name="estateId">معرف العقار المراد إضافته.</param>
        /// <returns>حالة العملية.</returns>
        [HttpPost("favorites/{estateId}")] // ده الـ Route للـ Endpoint: api/Products/favorites/{estateId}
        [Authorize] // هذا الـ Endpoint يتطلب تسجيل دخول
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // لو العقار مش موجود
        public async Task<IActionResult> AddToFavorites(int estateId)
        {
            var userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not identified.");

            // 1. التأكد أن العقار موجود في الداتا بيز
            var estateExists = await _context.Estates.AnyAsync(e => e.Id == estateId);
            if (!estateExists)
            {
                return NotFound("Estate not found.");
            }

            // 2. التأكد أن العقار مش موجود بالفعل في مفضلة المستخدم ده
            var exists = await _context.UserFavoriteEstates
                .AnyAsync(f => f.UserId == userId && f.EstateId == estateId);

            if (exists)
                return BadRequest("العقار موجود بالفعل في المفضلة.");

            // 3. إنشاء وإضافة الـ UserFavoriteEstate
            var favorite = new UserFavoriteEstate
            {
                UserId = userId,
                EstateId = estateId,
                AddedDate = DateTime.UtcNow
            };

            _context.UserFavoriteEstates.Add(favorite);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // لو حصل أي خطأ أثناء الحفظ (مثل مشكلة في قاعدة البيانات)
                return StatusCode(500, $"Error saving favorite: {ex.Message}");
            }

            return Ok("تمت الإضافة إلى المفضلة.");
        }

        /// <summary>
        /// 3. حذف عقار من المفضلة للمستخدم الحالي.
        /// DELETE: api/Products/favorites/{estateId}
        /// </summary>
        /// <param name="estateId">معرف العقار المراد حذفه.</param>
        /// <returns>حالة العملية.</returns>
        [HttpDelete("favorites/{estateId}")] // ده الـ Route للـ Endpoint: api/Products/favorites/{estateId}
        [Authorize] // هذا الـ Endpoint يتطلب تسجيل دخول
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromFavorites(int estateId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not identified.");
            }

            // البحث عن الـ Favorite Item المحدد
            var favorite = await _context.UserFavoriteEstates
                .FirstOrDefaultAsync(f => f.UserId == userId && f.EstateId == estateId);

            if (favorite == null)
                return NotFound("العقار غير موجود في المفضلة."); // العنصر ده مش موجود أصلاً في مفضلة المستخدم

            _context.UserFavoriteEstates.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok("تمت الإزالة من المفضلة.");
        }
    }


}


