using CarPark.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.Users.Controllers
{
    public class PersonelsController : Controller
    {
        private readonly IPersonelService _personelService;

        public PersonelsController(IPersonelService personelService)
        {
            _personelService = personelService;
        }

        public IActionResult GetPersonelsByAge()
        {
            var result = _personelService.GetPersonelsByAge();
            return View(result.Result);
        }
    }
}
