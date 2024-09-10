using AspNetCore.Identity.MongoDbCore.Models;
using CarPark.Business.Abstract;
using CarPark.Business.Concrete;
using CarPark.Core.Repository.Abstract;
using CarPark.Core.Settings;
using CarPark.DataAccess.Abstract;
using CarPark.DataAccess.Concrete;
using CarPark.DataAccess.Repository;
using CarPark.Entities.Concrete;
using CarPark.Users.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using System.Configuration;
using System.Globalization;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        //Serilog kullanarak bir Logger oluþturuldu
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log.txt")
        .WriteTo.Seq("http://localhost:5341/")
        .MinimumLevel.Information()
        .Enrich.WithProperty("ApplicationName", "CarPark.Users")
        .Enrich.WithMachineName()
        .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        // Authentication yapýlandýrmasý belirlendi.(cookie ile iþlem yapýlacak)
        builder.Services.AddAuthentication(option =>
        {
            option.DefaultScheme = IdentityConstants.ApplicationScheme;
            option.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies(o =>
        {
        });

        // Identity ile MongoDB kullanarak kimlik doðrulama ve yetkilendirme iþlemleri yapýlandýrýldý.
        builder.Services.AddIdentityCore<Personel>(option =>
        {
        })
            .AddRoles<MongoIdentityRole>()
            .AddMongoDbStores<Personel, MongoIdentityRole, Guid>(builder.Configuration.GetSection("MongoConnection:ConnectionString").Value, builder.Configuration.GetSection("MongoConnection:Database").Value)
            .AddSignInManager()
            .AddDefaultTokenProviders();

        // kimlik doðrulama çerezlerinin (cookies) nasýl yapýlandýrýlacaðý belirlendi.
        builder.Services.ConfigureApplicationCookie(option =>
        {
            option.Cookie.HttpOnly = true;
            option.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            option.LoginPath = "/Account/Login";
            option.SlidingExpiration = true;
        });

        //Dependency Injection yoluyla bir hizmeti uygulamanýn çeþitli yerlerinde kullanýlabilir hale getirildi. 
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepositoryBase<>));
        builder.Services.AddScoped<IPersonelDataAccess, PersonelDataAccess>();
        builder.Services.AddScoped<IPersonelService, PersonelManager>();

        builder.Services.AddControllersWithViews();

        // uygulamanýn localization içeriklerinin Resources klasöründen okumasýný saðlar 
        builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

        // Models'ta veri girilmeyince key deðil value deðeri gelsin istiyorum.Yani NameSurname_Value deðil "Adýnýz Soyadýnýz Zorunlu Alan" yazýsý gelsin istiyorum
        builder.Services.AddMvc()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
             {
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                  {
                      var assemblyName = new AssemblyName(typeof(SharedModelResource).GetTypeInfo().Assembly.FullName);
                      return factory.Create(nameof(SharedModelResource), assemblyName.Name);
                  };
            });

        //Uygulamanýn yapýlandýrma dosyasýndaki MongoDB baðlantý ayarlarý MongoSettings adlý sýnýfa aktarýldý
        builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoConnection"));

        // localization ayrýntýlý bir þekilde yapýlandýrýldý
        builder.Services.Configure<RequestLocalizationOptions>(opt =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("tr-TR"),
                new CultureInfo("fr-FR")
            };

            opt.DefaultRequestCulture = new RequestCulture("tr-TR");
            opt.SupportedCultures = supportedCultures;
            opt.SupportedUICultures = supportedCultures;

            opt.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            };
        });

        //Serilog uygulamanýn servis koleksiyonuna dahil edildi
        builder.Services.AddSerilog();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        // localization ayarlarýnýn nasýl uygulandýðý yazýldý
        var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(options.Value);

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}