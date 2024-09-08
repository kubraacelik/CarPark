using CarPark.Entities.Concrete;
using CarPark.Users.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace CarPark.Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            //localizer örneði
            //var say_Hello_value = _localizer["Say_Hello"];

            //var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            //Thread.CurrentThread.CurrentCulture = cultureInfo;
            //Thread.CurrentThread.CurrentUICulture = cultureInfo;

            //var say_Hello_value2 = _localizer["Say_Hello"];

            //serilog örneði
            //var customer = new Customer
            //{
            //    Id = 2,
            //    NameSurname = "Leyla Yýldýz",
            //    Age = 25
            //};

            //_logger.LogError("Customer'da bir hata oluþtu! {@customer}", customer);

            //veritabanýna baðlandý
            //amaç cities.json dosyasýný string yap ve cities modeli olarak veritabanýna ekle
            //var client = new MongoClient("mongodb+srv://kkbracelik92:m3GCIkXeoC64bUb2@cluster0.o9s5m.mongodb.net/");
            //var database = client.GetDatabase("CarParkDB");
            //var jsonString = System.IO.File.ReadAllText("cities.json");
            //var cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cities>>(jsonString);

            //var citiesCollection = database.GetCollection<City>("City");
            //foreach (var item in cities)
            //{
            //    var city = new City()
            //    {
            //        Id = ObjectId.GenerateNewId(),
            //        Name = item.name,
            //        Plate = item.plate,
            //        Latitude = item.latitude,
            //        Longitude = item.longitude,
            //        Counties = item.counties.ToList() // Burada `counties` listesinin `County` nesneleri içermesi gerekir.
            //    };

            //    citiesCollection.InsertOne(city);
            //}


            //var test = new Test()
            //{
            //    _Id = ObjectId.GenerateNewId(),
            //    NameSurname = "Kübra Çelik",
            //    Age = 23,
            //    AddressList = new List<Address>() {
            //        new Address
            //        {
            //            Title="Ev Adresim",
            //            Description = "Gaziantep/Þehitkamil"
            //        },
            //        new Address
            //        {
            //            Title="Yurt",
            //            Description="Zonguldak/Merkez"
            //        }
            //    }
            //};

            //collection.InsertOne(test);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            return View();
        } 
    }
}
