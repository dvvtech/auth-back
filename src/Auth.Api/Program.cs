using Auth.Api.AppStart;
using Auth.Api.AppStart.Extensions;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder);
startup.Initialize();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.ApplyCors();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
