using E_knjiznica.Data;
using E_knjiznica.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Dodaj usluge u aplikaciju
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // ✅ Ovo registrira Razor Pages, potrebno za ASP.NET Identity
builder.Services.AddHttpClient<OpenLibraryService>();



// Konfiguracija baze podataka
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj ASP.NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<LibraryDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Konfiguracija HTTP zahtjeva
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ✅ Dodana autentifikacija
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.MapRazorPages(); // ✅ Omogućuje korištenje Identity stranica (login, register)

app.Run();
