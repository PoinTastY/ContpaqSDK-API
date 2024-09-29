using Application.DTOs;
using Application.UseCases.SDK;
using Application.UseCases.SDK.Documentos;
using Application.UseCases.SQL.Documentos;
using Application.UseCases.SQL.Movimientos;
using Application.UseCases.SQL.Productos;
using Domain.Interfaces;
using Domain.Interfaces.Repos;
using Domain.SDK_Comercial;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseWindowsService();
//builder.Services.AddWindowsService();//to use it as a windows service

var sdkSettings = LoadSettings();

var logFilePath = "C:\\Stare-y\\ContpaqSDK-API\\log.txt";
string directoryPath = Path.GetDirectoryName(logFilePath);

// Crea el directorio si no existe
if (!Directory.Exists(directoryPath))
{
    Directory.CreateDirectory(directoryPath);
}

var logger = new Logger(logFilePath);

// Add services to the container.
builder.Services.AddSingleton<Domain.Interfaces.Services.ILogger>(provider => logger);
builder.Services.AddSingleton<SDKSettings>(provider => sdkSettings);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repositories
builder.Services.AddDbContext<ContpaqiSQLContext>(options =>
{
    options.UseSqlServer(sdkSettings.SQLConnectionString);
});
builder.Services.AddSingleton<SDKRepo>();
builder.Services.AddSingleton<ISDKRepo>(sp => sp.GetRequiredService<SDKRepo>());
builder.Services.AddSingleton<IDocumentRepo, DocumentRepo>();
builder.Services.AddSingleton<IProductRepo, ProductRepo>();
builder.Services.AddSingleton<IMovimientoRepo, MovimientoRepo>();

//UseCases
#region SDK Services

#region Documentos

builder.Services.AddTransient<AddDocumentWithMovementSDKUseCase>();
builder.Services.AddTransient<SetDocumentoImpresoSDKUseCase>();
builder.Services.AddTransient<GetDocumentByIdSDKUseCase>();
builder.Services.AddTransient<GetDocumedntByConceptoFolioAndSerieSDKUseCase>();

#endregion

builder.Services.AddTransient<TestSDKUseCase>();

#endregion

#region SQL Services

#region Documentos

builder.Services.AddTransient<GetPedidosSQLUseCase>();

#endregion

#region Movimientos

builder.Services.AddTransient<GetIdsMovimientosByIdDocumentoSQLUseCase>();
builder.Services.AddTransient<GetMovimientosByIdDocumentoSQLUseCase>();

#endregion

#region Productos

builder.Services.AddTransient<GetAllProductsSQLUseCase>();
builder.Services.AddTransient<GetProductByIdSQLUseCase>();
builder.Services.AddTransient<GetProductoByCodigoSQLUseCase>();
builder.Services.AddTransient<GetProductosByIdsCPESQLUseCase>();
builder.Services.AddTransient<GetProductosByIdsSQLUseCase>();

#endregion

#endregion

//Other configs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
//app.UseHttpsRedirection();

var lifetime = app.Lifetime;
lifetime.ApplicationStopping.Register(async () =>
{
    logger.Log("Application is stopping");
    var sdkRepo = app.Services.GetRequiredService<SDKRepo>();
    await sdkRepo.DisposeSDK();
});

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
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

#region SDK Endpoints

app.MapPost("/addDocumentWithMovementSDK/", async (AddDocumentWithMovementSDKUseCase useCase, DocumentDTO documento) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para agregar documento con movimiento.");

        var documentDTO = await useCase.Execute(documento);


        logger.Log($"Documento agregado con �xito. Id: {documentDTO.CIDDOCUMENTO}, Folio {documentDTO.aFolio}");

        return Results.Ok(new ApiResponse{ Message = "Documento agregado con �xito", Data = documentDTO, Success = true });
    }
    catch (Exception ex)
    {
        logger.Log($"Error al agregar el documento: {ex.Message}");
        return Results.BadRequest(new ApiResponse{ Message = "Error al agregar el documento", Error = ex.Message , Success = false});
    }
})
.WithName("AddDocumentWithMovementSDK")
.WithDescription("Agrega un documento con movimiento a la base de datos de Contpaq SDK")
.WithOpenApi();

app.MapPut("/setDocumentoImpresoSDK/{idDocumento}", async (SetDocumentoImpresoSDKUseCase useCase, int idDocumento) =>
{
    try
    {
        // Ejecutamos el caso de uso con el documento proporcionado
        await useCase.Execute(idDocumento);
        return Results.Ok(new ApiResponse{ Message = "Documento marcado como impreso" , Success = true });
    }
    catch (Exception ex)
    {
        logger.Log($"Error al marcar el documento como impreso: {ex.Message}");
        return Results.BadRequest(new ApiResponse{ Message = "Error al marcar el documento como impreso", Error = ex.Message, Success = false });
    }
})
.WithName("SetDocumentoImpresoSDK")
.WithDescription("Marca un documento como impreso en la base de datos de Contpaq SDK")
.WithOpenApi();

app.MapGet("/getDocumentByIdSDK/{idDocumento}", async (GetDocumentByIdSDKUseCase useCase, int idDocumento) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener documento por id.");
        var document = await useCase.Execute(idDocumento);
        logger.Log($"Documento obtenido con �xito. Id: {idDocumento}, Folio: {document.aFolio}");

        var apiResponse = new ApiResponse{ Message = "Documento obtenido con �xito", Data = document , Success = true};

        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener el documento: {ex.Message}");
        return Results.BadRequest(new ApiResponse{ Message = $"Error al obtener el documento: {ex.Message}", Error = ex.Message,  Success = true});
    }
})
.WithName("GetDocumentByIdSDK")
.WithDescription("Obtiene un documento por su id en la base de datos de Contpaq SDK")
.WithOpenApi();

app.MapGet("/getDocumentByConceptoFolioAndSerieSDK/{codConcepto}/{serie}/{folio}", async (GetDocumedntByConceptoFolioAndSerieSDKUseCase useCase, string codConcepto, string serie, string folio) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener documento por concepto, serie y folio.");
        var document = await useCase.Execute(codConcepto, serie, folio);
        logger.Log($"Documento obtenido con �xito. Folio: {document.aFolio}, Concepto: {document.aCodConcepto}, Serie: {document.aSerie}");

        var apiResponse = new ApiResponse{ Message = "Documento obtenido con �xito", Data = document , Success = true};
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener el documento: {ex.Message}");
        return Results.BadRequest(new ApiResponse{ Message = $"Error al obtener el documento: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetDocumentByConceptoFolioAndSerieSDK")
.WithDescription("Obtiene un documento por su concepto, serie y folio en la base de datos de Contpaq SDK")
.WithOpenApi();

app.MapGet("/isServiceWorkingSDK", async (TestSDKUseCase useCase) =>
{
    logger.Log("Se pregunto si la api esta chambeando");
    try
    {
        await useCase.Execute();
        var apiResponse = new ApiResponse { Message = "ContpaqSDK-API is working", Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al preguntar si la api esta chambeando: {ex.Message}");
        return Results.BadRequest(new ApiResponse { Message = "Parece que el SDK no esta trabajando correctamente.", Error = ex.Message, Success = false });
    }
})
.WithName("IsServiceWorkingSDK")
.WithDescription("Prueba si el SDK esta trabajando correctamente")
.WithOpenApi();

#endregion

#region SQL Endpoints

app.MapGet("/getPedidosByFechaSerieSQL/{fechaInicio}/{fechaFin}/{serie}", async (GetPedidosSQLUseCase useCase, DateTime fechaInicio, DateTime fechaFin, string serie) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener pedidos CPE.");
        var documents = await useCase.Execute(fechaInicio, fechaFin, serie);
        logger.Log($"Pedidos obtenidos con �xito. Cantidad: {documents.Count}");

        var apiResponse = new ApiResponse { Message = "Pedidos obtenidos con �xito", Data = documents, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los pedidos CPE: {ex.Message}");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los pedidos CPE: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetPedidosByFechaSerieSQL")
.WithDescription("Obtiene los documentos ")
.WithOpenApi();

app.MapGet("getIdsMovimientosByIdDocumentoSQL/{idDocumento}", async (GetIdsMovimientosByIdDocumentoSQLUseCase useCase, int idDocumento) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener los ids de los movimientos por id de documento.");
        var ids = await useCase.Execute(idDocumento);
        logger.Log($"Ids de movimientos obtenidos con �xito. Cantidad: {ids.Count}");

        var apiResponse = new ApiResponse { Message = "Ids de movimientos obtenidos con �xito", Data = ids, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los ids de los movimientos: {ex.Message}");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los ids de los movimientos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetList of ids by document id")
.WithDescription("Obtiene los movimientos relacionados al movimiento")
.WithOpenApi();

app.MapGet("getProductosByIdsSQL/", async (GetProductosByIdsSQLUseCase useCase, List<int> ids) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener productos por ids.");
        var products = await useCase.Execute(ids);
        logger.Log($"Productos obtenidos con �xito. Cantidad: {products.Count}");

        var apiResponse = new ApiResponse { Message = "Productos obtenidos con �xito", Data = products, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los productos: {ex.Message}");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los productos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetProductosByIds")
.WithDescription("Gets the list of products by ids")
.WithOpenApi();

#endregion

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
