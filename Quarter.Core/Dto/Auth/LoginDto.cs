using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Dtos.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
