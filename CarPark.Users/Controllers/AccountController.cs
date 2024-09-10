using AspNetCore.Identity.MongoDbCore.Models;
using CarPark.Entities.Concrete;
using CarPark.Models.RequestModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.Users.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Personel> _userManager;
        private readonly SignInManager<Personel> _signInManager;
        private readonly RoleManager<MongoIdentityRole> _roleManager;

        public AccountController(UserManager<Personel> userManager, SignInManager<Personel> signInManager, RoleManager<MongoIdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Veri gönderimi veya işlem yapılmaz, sadece sayfa gösterilir.(Add View işlemi burda yapılır)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Form gönderildiğinde çalışır ve kullanıcıyı kaydetme işlemi yapar.
        [HttpPost]
        public async Task<IActionResult> Register(RegisterCreateModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new Personel
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = "05555555555"
                };   

                var result = await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded) 
                {
                    var role = new MongoIdentityRole
                    {
                        Name = "normal",
                        NormalizedName = "NORMAL"
                    };

                    var resultRole = await _roleManager.CreateAsync(role);

                    await _userManager.AddToRoleAsync(user, "normal");

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToLocal(returnUrl);
                }

            }
            return View(model);
        }

        // Güvenli yönlendirmeleri yönetir
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        // Oturum açma formunu görüntüler.(Add View burada olacak)
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Kullanıcının kimlik bilgilerini doğrular ve başarılıysa oturum açmasını sağlar.
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
            }
                return View(model);
        }

        // Kullanıcıyı uygulamadan oturumu kapatıp ana sayfaya yönlendirir
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Kullanıcının yetkilendirilmediği sayfaya erişmeye çalıştığında gösterilecek hata sayfası 
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
