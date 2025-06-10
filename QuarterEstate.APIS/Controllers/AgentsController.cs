using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // ✨ إصلاح 1: لإصلاح مشكلة ToListAsync
using Quarter.Core.Dto;
using Quarter.Core.Entites;
using Quarter.Repostory.Data.Context;
using System;
using System.Collections.Generic;   // ✨ إصلاح 2: لإصلاح مشكلة IEnumerable
using System.Linq;                  // ✨ إصلاح 3: لإصلاح مشكلة Select
using System.Threading.Tasks;
using static Quarter.Core.Dto.AgentDto;       // ✨ إصلاح 4: لإصلاح مشكلة Task

namespace QuarterEstate.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        // اسم الكلاس هنا صحيح كما هو في مشروعك
        private readonly QuarterDbContexts _context;

        public AgentsController(QuarterDbContexts context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgentDto>>> GetAllAgents()
        {
            // الخطوة 1: جلب كل الوكلاء مع تضمين قائمة العقارات المرتبطة بكل وكيل
            var agents = await _context.Agents
                                       .Include(a => a.Estates) // <-- هذا هو التعديل الأساسي
                                       .ToListAsync();

            // الخطوة 2: تحويل البيانات إلى DTOs
            var agentDtos = agents.Select(agent => new AgentDto
            {
                Id = agent.Id,
                Name = agent.Name,
                PhoneNumber = agent.PhoneNumber,
                ImageUrl = agent.ImageUrl,
                Description = agent.Description,
                DateCreated = agent.DateCreated,
                AppUserId = agent.AppUserId,

                // ✨ نقوم أيضًا بتحويل قائمة العقارات الخاصة بكل وكيل
                Estates = agent.Estates.Select(estate => new EstateDto
                {
                    Id = estate.Id,
                    Name = estate.Name,
                    Price = estate.Price
                }).ToList()

            }).ToList();

            return Ok(agentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgentDto>> GetAgentById(int id)
        {
         
            var agent = await _context.Agents
                                      .Include(a => a.Estates) // <-- هذا هو السطر السحري
                                      .FirstOrDefaultAsync(a => a.Id == id);

            if (agent == null)
            {
                return NotFound();
            }

            // 2. نقوم بتحويل (mapping) بيانات الوكيل والعقارات إلى DTOs
            var agentDto = new AgentDto
            {
                Id = agent.Id,
                Name = agent.Name,
                PhoneNumber = agent.PhoneNumber,
                ImageUrl = agent.ImageUrl,
                Description = agent.Description,
                DateCreated = agent.DateCreated,
                AppUserId = agent.AppUserId,
                // نقوم بتحويل قائمة العقارات أيضًا
                Estates = agent.Estates.Select(estate => new EstateDto
                {
                    Id = estate.Id,
                   
                }).ToList()
            };

            return Ok(agentDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAgent(CreateAgentDto agentDto)
        {
            var agent = new Agent
            {
                Name = agentDto.Name,
                PhoneNumber = agentDto.PhoneNumber,
                ImageUrl = agentDto.ImageUrl,
                Description = agentDto.Description, // ✨ إصلاح 6: تمت إضافة الوصف هنا
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();

            var createdAgentDto = new AgentDto
            {
                Id = agent.Id,
                Name = agent.Name,
                PhoneNumber = agent.PhoneNumber,
                ImageUrl = agent.ImageUrl,
                Description = agent.Description, // ✨ إصلاح 7: تمت إضافة الوصف هنا أيضاً
                DateCreated = agent.DateCreated
            };

            return CreatedAtAction(nameof(GetAgentById), new { id = agent.Id }, createdAgentDto);
        }
    }
}