using CarPark.Business.Abstract;
using CarPark.Business.Concrete;
using CarPark.Core.Repository.Abstract;
using CarPark.Core.Settings;
using CarPark.DataAccess.Abstract;
using CarPark.DataAccess.Concrete;
using CarPark.DataAccess.Repository;
using CarPark.Users.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;
using NuGet.Packaging.Core;
using Serilog;
using System.Configuration;
using System.Globalization;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        //Serilog kullanarak bir Logger olu�turuldu
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log.txt")
        .WriteTo.Seq("http://localhost:5341/")
        .MinimumLevel.Information()
        .Enrich.WithProperty("ApplicationName", "CarPark.Users")
        .Enrich.WithMachineName()
        .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        //Dependency Injection yoluyla bir hizmeti uygulaman�n �e�itli yerlerinde kullan�labilir hale getirildi. 
        builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepositoryBase<>));
        builder.Services.AddScoped<IPersonelDataAccess, PersonelDataAccess>();
        builder.Services.AddScoped<IPersonelService, PersonelManager>();

        builder.Services.AddControllersWithViews();

        // uygulaman�n localization i�eriklerinin Resources klas�r�nden okumas�n� sa�lar 
        builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

        // Models'ta veri girilmeyince key de�il value de�eri gelsin istiyorum.Yani NameSurname_Value de�il "Ad�n�z Soyad�n�z Zorunlu Alan" yaz�s� gelsin istiyorum
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

        //Uygulaman�n yap�land�rma dosyas�ndaki MongoDB ba�lant� ayarlar� MongoSettings adl� s�n�fa aktar�ld�
        builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoConnection"));

        // localization ayr�nt�l� bir �ekilde yap�land�r�ld�
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

        //Serilog uygulaman�n servis koleksiyonuna dahil edildi
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

        app.UseAuthorization();

        // localization ayarlar�n�n nas�l uyguland��� yaz�ld�
        var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(options.Value);

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}