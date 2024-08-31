using CarPark.Users.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;

namespace CarPark.Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //veritabanýna baðlandý
            var client = new MongoClient("mongodb+srv://kkbracelik92:IKqVw1VFLOySqv65@carparkcluster.kvpxp.mongodb.net/");
            var database = client.GetDatabase("CarParkDB");
            var collection = database.GetCollection<Test>("Test");

            var test = new Test()
            {
                _Id = ObjectId.GenerateNewId(),
                NameSurname = "Kübra Çelik",
                Age = 23,
                AddressList = new List<Address>() {
                    new Address
                    {
                        Title="Ev Adresim",
                        Description = "Gaziantep/Þehitkamil"
                    },
                    new Address
                    {
                        Title="Yurt",
                        Description="Zonguldak/Merkez"
                    }
                }
            };

            collection.InsertOne(test);
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
    }
}
