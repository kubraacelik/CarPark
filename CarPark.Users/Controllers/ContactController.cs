using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CarPark.Users.Controllers
{
    public class ContactController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public ContactController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            var welcome_value = _localizer["Welcome"];
            return View();
        }
    }
}
