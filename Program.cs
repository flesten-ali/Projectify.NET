using Practice.Db;
using Practice.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
 



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(

//    builder.Configuration.GetConnectionString("DefaultConnection")
//    ));

builder.Services.AddDbContext<AuthDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("AuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();


// Passward Policies
builder.Services.Configure<IdentityOptions>(

 options =>
 {
     options.Password.RequireDigit = true;
     options.Password.RequireNonAlphanumeric = true;
     options.Password.RequireLowercase = true;
     options.Password.RequireUppercase = true;


 }

);

//the updates happen when you save and refresh the page while you are running it 
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
// Ineject Email Service
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<EmailSender>();
//Token Confirmaation password


///////////
builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);
////////////////////////////

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.SignIn.RequireConfirmedEmail = true;
//});
builder.Services.Configure<DataProtectionTokenProviderOptions>
   (options => options.TokenLifespan = TimeSpan.FromMinutes(10));





///////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
