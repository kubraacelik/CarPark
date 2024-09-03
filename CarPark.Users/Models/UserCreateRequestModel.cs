using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarPark.Users.Models
{
    public class UserCreateRequestModel
    {
        // {0} = DisplayName'e denk geliyor

        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("NameSurname")]
        public string NameSurname { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("JobTitle")]
        public string JobTitle { get; set; }
    }
}
