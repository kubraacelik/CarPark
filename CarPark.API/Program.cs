using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        //appsettings.json dosyasýný okumak için yazýldý
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        //Serilog'u appsettings.json gibi bir yapýlandýrma dosyasýndaki ayarlarla yapýlandýrmak için yazýldý
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}