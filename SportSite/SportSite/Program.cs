using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using SportSite.Models.Db;
using SportSite.Models.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string connection =  builder.Configuration.GetConnectionString("Default");
builder.Logging.ClearProviders();
builder.Logging.AddConsole();  //Logger write in the Console
builder.Logging.AddDebug();  //Logger write in the Debug
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connection));
builder.Services.AddSignalR();  //SignalR
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();  // Blazor
builder.Services.AddServerSideBlazor();  // Blazor
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>  //CookieAuthenticationOptions
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/");
        });
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapHub<MessageHub>("/message");
app.MapRazorPages();
app.MapBlazorHub();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
