using EricTest.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Register auth services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<KeyService>();
builder.Services.AddScoped<IssuerProfileService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<AchievementService>();
builder.Services.AddScoped<CredentialIssuerService>();
// Add authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var profilesPath = Path.Combine(builder.Environment.ContentRootPath, "profiles");
Directory.CreateDirectory(profilesPath);
var wellKnownPath = Path.Combine(builder.Environment.WebRootPath, ".well-known");
Directory.CreateDirectory(wellKnownPath);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "EricTest API", Version = "v1" });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EricTest API v1"));
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();