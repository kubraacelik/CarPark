using Serilog;

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
        .Enrich.WithProperty("ApplicationName","CarPark.Users")
        .Enrich.WithMachineName()
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