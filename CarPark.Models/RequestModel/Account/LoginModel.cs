using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Models.RequestModel.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Zorunlu")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Zorunlu")]
        [StringLength(100, ErrorMessage = "Minumum 6 karakter olmak zorunda", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
