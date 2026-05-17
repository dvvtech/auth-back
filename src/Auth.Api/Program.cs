using Auth.Api.AppStart;
using Auth.Api.AppStart.Extensions;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

var keyBytes = RandomNumberGenerator.GetBytes(64);
var rty = Convert.ToBase64String(keyBytes);

var startup = new Startup(builder);
startup.Initialize();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "AllowSpecificOrigin");
//app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseMiddleware<CookieTokenMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
