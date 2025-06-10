using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dto
{
    public class AgentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }

        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public List<EstateDto> Estates { get; set; }
        public string? AppUserId { get; set; }
        public class CreateAgentDto
        {
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string ImageUrl { get; set; }
            // تمت إعادة هذه الخاصية
            public string Description { get; set; }
           
        }
    }
}
