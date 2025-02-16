using Core.Domain.Interfaces.Repositories.SQL;
using Infrastructure.Repositories;
using Pedidos_CPE.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWindowsService();

// Add services to the container.
builder = DependencyServices.ConfigureServices(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//start the SDK
using (var scope = app.Services.CreateScope())
{
    SDKRepo sdkRepo = scope.ServiceProvider.GetRequiredService<SDKRepo>();
    //get a logger instance
    var logger = scope.ServiceProvider.GetRequiredService<Core.Domain.Interfaces.Services.ILogger>();
    try
    {
        await sdkRepo.InicializarSDKAsync();
    }
    catch (Exception e)
    {
        logger.Log(e.Message);
        throw;
    }
}

// Dispose the SDK when the application stops
var lifetime = app.Lifetime;
lifetime.ApplicationStopping.Register(async () =>
{
    var sdkRepo = app.Services.GetRequiredService<SDKRepo>();
    await sdkRepo.TerminaSDK();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
