using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dto.Auth
{
    public class UpdateUserDto
    {
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
