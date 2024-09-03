using CarPark.Users.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace CarPark.Users.Controllers
{
    public class UserController : Controller
    {
        private readonly IStringLocalizer<UserController> _localizer;
        public UserController(IStringLocalizer<UserController> localizer)
        {
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            var nameSurnameValue = _localizer["NameSurname"];

            return View();
        }

        // UserCreateRequestModel sınıfının propsları ile sayfa açıldı
        public IActionResult Create() 
        {
            return View();
        }

        //UserCreateRequestModel'daki Required[] yazısının gelmesi için eklendi
        [HttpPost]
        public IActionResult Create(UserCreateRequestModel request)
        {
            return View(request);
        }
    }
}
