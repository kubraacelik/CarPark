using CarPark.Core.Repository.Abstract;
using CarPark.Entities.Concrete;
using CarPark.Users.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace CarPark.Users.Controllers
{
    public class UserController : Controller
    {
        private readonly IStringLocalizer<UserController> _localizer;
        private readonly IRepository<Personel> _personelRepository;

        public UserController(IStringLocalizer<UserController> localizer, IRepository<Personel> personelRepository)
        {
            _localizer = localizer;
            _personelRepository = personelRepository;
        }
        public IActionResult Index()
        {
            var nameSurnameValue = _localizer["NameSurname"];

            return View();
        }

        public IActionResult Create()
        {
            //var result = _personelRepository.InsertOne(new Personel
            //{
            //    Email = "kkbra.celik92@gmail.com",
            //    Password = "1234",
            //    CreatedDate = DateTime.Now,
            //    UserName = "kkbra.celik"
            //});

            //var result2 = _personelRepository.InsertOneAsync(new Personel
            //{
            //    Email = "kubracelik@gmail.com",
            //    Password = "kubra123",
            //    CreatedDate = DateTime.Now,
            //    UserName = "kubracelik"
            //});

            //var result3 = _personelRepository.InsertMany(new List<Personel>
            //{
            //    new Personel
            //    {
            //        Email = "azraakin@gmail.com",
            //        Password = "azra321",
            //        CreatedDate = DateTime.Now,
            //        UserName = "azraakin"
            //    },
            //    new Personel
            //    {
            //        Email = "berkgunes@gmail.com",
            //        Password = "berk456",
            //        CreatedDate = DateTime.Now,
            //        UserName = "berkgunes"
            //    }
            //});

            //var result4 = _personelRepository.AsQueryable();

            //var result5 = _personelRepository.GetById("66db187bcc3fbb01cec0f031");

            //var result6 = _personelRepository.DeleteOne(x => x.Email.Contains("raak"));

            var result7 = _personelRepository.GetById("66db19da556ad7e25bf64c80");
            result7.Entity.Email = "berkgunes74@gmail.com";
            result7.Entity.Password = "berk45686";
            result7.Entity.UserName = "berkgunes74";
            var result8 = _personelRepository.ReplaceOne(result7.Entity, result7.Entity.Id.ToString());

 
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
