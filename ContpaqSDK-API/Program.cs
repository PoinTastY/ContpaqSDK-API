using Application.DTOs;
using Application.UseCases;
using Domain.Interfaces;
using Domain.SDK_Comercial;
using Infrastructure.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var sdkSettings = LoadSettings();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISDKRepo, SDKRepo>();
builder.Services.AddSingleton(sdkSettings);
builder.Services.AddTransient<AddDocumentWithMovementSDKUseCase>();
builder.Services.AddTransient<SetDocumentoImpresoSDKUseCase>();

var app = builder.Build();

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
        // Ejecutamos el caso de uso con el documento y movimiento proporcionados
        var idDocumento = await useCase.Execute(documento);

        return Results.Ok(new { Message = "Documento agregado con éxito", IdDocumento = idDocumento });
    }
    catch (Exception ex)
    {
        // Devolvemos un error en caso de excepción
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
        // Devolvemos un error en caso de excepción
        return Results.BadRequest(new { Message = "Error al marcar el documento como impreso", Error = ex.Message });
    }
})
.WithName("SetDocumentoImpreso")
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
