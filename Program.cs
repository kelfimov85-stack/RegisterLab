using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegRoma.Data;
using RegRoma.Services;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.Services.AddControllers();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("AppDb");
});
builder.Services.AddScoped<PasswordServices>();
builder.Services.AddScoped<JwtServices>();

var jwtKey = builder.Configuration["JwtKey"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
   options.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuer = true, //надо проверить
       ValidateAudience = true,
       ValidateIssuerSigningKey = true,
       ValidateLifetime = true,
       ValidIssuer = builder.Configuration["Jwt:Issuer"], //что проверить
       ValidAudience = builder.Configuration["Jwt:Audience"],
       IssuerSigningKey = new SymmetricSecurityKey(keyBytes)//проверка самого токена
   };
});

app.UseAuthentication();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
