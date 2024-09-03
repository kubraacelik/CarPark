using CarPark.Users.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;
using Serilog;
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

            //opt.RequestCultureProviders = new[]
            //{
            //    new RouteDataRequestCultureProvider
            //    {
            //        Options = opt
            //    }
            //};
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