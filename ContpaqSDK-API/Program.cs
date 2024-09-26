using Application.DTOs;
using Application.UseCases;
using Domain.Interfaces;
using Domain.SDK_Comercial;
using Infrastructure.Repositories;
using Infrastructure.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var sdkSettings = LoadSettings();

var logFilePath = "C:\\Stare-y\\ContpaqSDK-API\\log.txt";
string directoryPath = Path.GetDirectoryName(logFilePath);

// Crea el directorio si no existe
if (!Directory.Exists(directoryPath))
{
    Directory.CreateDirectory(directoryPath);
}

var logger = new Logger(logFilePath);
builder.Services.AddSingleton<Domain.Interfaces.ILogger>(provider => logger);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SDKRepo>();
builder.Services.AddSingleton<ISDKRepo>(sp => sp.GetRequiredService<SDKRepo>());
builder.Services.AddSingleton(sdkSettings);
builder.Services.AddTransient<AddDocumentWithMovementSDKUseCase>();
builder.Services.AddTransient<SetDocumentoImpresoSDKUseCase>();

var app = builder.Build();

//start the SDK
using (var scope = app.Services.CreateScope())
{
    try
    {
        var sdkRepo = scope.ServiceProvider.GetRequiredService<SDKRepo>();
        await sdkRepo.InitializeAsync();
    }
    catch (Exception ex)
    {
        logger.Log($"Error al inicializar el SDK: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/addDocumentWithMovement", async (AddDocumentWithMovementSDKUseCase useCase, DocumentDTO documento) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para agregar documento con movimiento.");

        var folioAndIdDocumento = await useCase.Execute(documento);
        var idDocumento = folioAndIdDocumento.Keys.First();


        logger.Log($"Documento agregado con éxito. Id: {idDocumento}, Folio {folioAndIdDocumento[idDocumento]}");

        return Results.Ok(new { Message = "Documento agregado con éxito", FolioDocumento = folioAndIdDocumento[idDocumento], IdDocumento = idDocumento });
    }
    catch (Exception ex)
    {
        logger.Log($"Error al agregar el documento: {ex.Message}");
        return Results.BadRequest(new { Message = "Error al agregar el documento", Error = ex.Message });
    }
})
.WithName("AddDocumentWithMovement")
.WithOpenApi();

app.MapPost("/setDocumentoImpreso{idDocumento}", async (SetDocumentoImpresoSDKUseCase useCase, int idDocumento) =>
{
    try
    {
        // Ejecutamos el caso de uso con el documento proporcionado
        await useCase.Execute(idDocumento);
        return Results.Ok(new { Message = "Documento marcado como impreso" });
    }
    catch (Exception ex)
    {
        logger.Log($"Error al marcar el documento como impreso: {ex.Message}");
        return Results.BadRequest(new { Message = "Error al marcar el documento como impreso", Error = ex.Message });
    }
})
.WithName("SetDocumentoImpreso")
.WithOpenApi();

app.MapGet("/isServiceWorking", () =>
{
    logger.Log("Se pregunto si la api esta chambeando");
    return Results.Ok("ContpaqSDK-API is working");
})
.WithName("IsServiceWorking")
.WithOpenApi();

app.Run();

SDKSettings LoadSettings()
{
    try
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "SDKSettings.json");
        if (!File.Exists(jsonPath))
        {
            throw new Exception($"SDKSettings.json not found on path: {jsonPath}");
        }

        string json = File.ReadAllText(jsonPath);
        if (string.IsNullOrEmpty(json))
        {
            throw new Exception("SDKSettings.json is empty");
        }
        else
        {
            return JsonSerializer.Deserialize<SDKSettings>(json);
        }
    }
    catch (Exception)
    {
        throw;
    }
}
