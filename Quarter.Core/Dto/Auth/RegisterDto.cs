using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dtos.Auth
{
    public class RegisterDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "password     ")]
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


    }
}
