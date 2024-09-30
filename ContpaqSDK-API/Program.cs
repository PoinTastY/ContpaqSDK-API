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
using Microsoft.AspNetCore.Mvc;
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
    options.UseSqlServer(sdkSettings.SQLConnectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null));
});
builder.Services.AddSingleton<SDKRepo>();
builder.Services.AddSingleton<ISDKRepo>(sp => sp.GetRequiredService<SDKRepo>());
builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IMovimientoRepo, MovimientoRepo>();

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

builder.Services.AddTransient<GetPedidosSQLCPEUseCase>();

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
        logger.Log($"Error al inicializar el SDK: {ex.Message} (Inner: {ex.InnerException})");
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


        logger.Log($"Documento agregado con éxito. Id: {documentDTO.CIDDOCUMENTO}, Folio {documentDTO.aFolio}");

        return Results.Ok(new ApiResponse{ Message = "Documento agregado con éxito", Data = documentDTO, Success = true });
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
        logger.Log($"Documento obtenido con éxito. Id: {idDocumento}, Folio: {document.aFolio}");

        var apiResponse = new ApiResponse{ Message = "Documento obtenido con éxito", Data = document , Success = true};

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
        logger.Log($"Documento obtenido con éxito. Folio: {document.aFolio}, Concepto: {document.aCodConcepto}, Serie: {document.aSerie}");

        var apiResponse = new ApiResponse{ Message = "Documento obtenido con éxito", Data = document , Success = true};
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

app.MapGet("/getPedidosByFechaSerieCPESQL/{fechaInicio}/{fechaFin}/{serie}", async (GetPedidosSQLCPEUseCase useCase, DateTime fechaInicio, DateTime fechaFin, string serie) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener pedidos CPE.");
        var documents = await useCase.Execute(fechaInicio, fechaFin, serie);
        logger.Log($"Pedidos obtenidos con éxito. Cantidad: {documents.Count}");

        var apiResponse = new ApiResponse { Message = "Pedidos obtenidos con éxito", Data = documents, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los pedidos CPE: {ex.Message} (Inner: {ex.InnerException})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los pedidos CPE: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetPedidosByFechaSerieCPESQL")
.WithDescription("Obtiene los documentos ")
.WithOpenApi();

app.MapGet("getIdsMovimientosByIdDocumentoSQL/{idDocumento}", async (GetIdsMovimientosByIdDocumentoSQLUseCase useCase, int idDocumento) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener los ids de los movimientos por id de documento.");
        var ids = await useCase.Execute(idDocumento);
        logger.Log($"Ids de movimientos obtenidos con éxito. Cantidad: {ids.Count}");

        var apiResponse = new ApiResponse { Message = "Ids de movimientos obtenidos con éxito", Data = ids, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los ids de los movimientos: {ex.Message} (Inner: {ex.InnerException})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los ids de los movimientos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetList of ids by document id")
.WithDescription("Obtiene los movimientos relacionados al movimiento")
.WithOpenApi();

app.MapGet("getMovimientosByIdDocumentoSQL/{idDocumento}", async (GetMovimientosByIdDocumentoSQLUseCase useCase, int idDocumento) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener los ids de los movimientos por id de documento.");
        var ids = await useCase.Execute(idDocumento);
        logger.Log($"Ids de movimientos obtenidos con éxito. Cantidad: {ids.Count}");

        var apiResponse = new ApiResponse { Message = "Ids de movimientos obtenidos con éxito", Data = ids, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los ids de los movimientos: {ex.Message} (Inner: {ex.InnerException})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los ids de los movimientos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetList of movements by document id")
.WithDescription("Obtiene los movimientos relacionados al documento")
.WithOpenApi();

app.MapPost("getProductosByIdsSQL/", async (GetProductosByIdsSQLUseCase useCase, List<int> ids) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener productos por ids.");
        var products = await useCase.Execute(ids);
        logger.Log($"Productos obtenidos con éxito. Cantidad: {products.Count}");

        var apiResponse = new ApiResponse { Message = "Productos obtenidos con éxito", Data = products, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los productos: {ex.Message} (Inner: {ex.InnerException}) (Id's recibidos: {string.Join(", ", ids)})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los productos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetProductosByIds")
.WithDescription("Gets the list of products by ids")
.WithOpenApi();

app.MapPost("getProductosByIdsCPESQL/", async (GetProductosByIdsCPESQLUseCase useCase, List<int> ids) =>
{
    try
    {
        logger.Log($"Recibiendo solicitud para obtener {ids.Count} productos por ids CPE.");
        var products = await useCase.Execute(ids);
        logger.Log($"Productos obtenidos con éxito. Cantidad: {products.Count}");

        var apiResponse = new ApiResponse { Message = "Productos obtenidos con éxito", Data = products, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los productos: {ex.Message} (Inner: {ex.InnerException}) (Id's recibidos: {string.Join(", ", ids)})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los productos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetProductosByIdsCPESQL")
.WithDescription("Gets the list of products by ids, filtering cpe's rules")
.WithOpenApi();

app.MapGet("getProductoByIdSQL/{idProducto}", async (GetProductByIdSQLUseCase useCase, int idProducto) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener un producto por su id.");
        var product = await useCase.Execute(idProducto);
        logger.Log($"Producto obtenido con éxito. Id: {product.CIDPRODUCTO}");
        var apiResponse = new ApiResponse { Message = "Producto obtenido con éxito", Data = product, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener el producto: {ex.Message} (Inner: {ex.InnerException})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener el producto: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetProductById")
.WithDescription("Obtiene un producto por su id")
.WithOpenApi();

app.MapPost("getPrductosByIdsCPESQL/", async (GetProductosByIdsCPESQLUseCase useCase, List<int> ids) =>
{
    try
    {
        logger.Log("Recibiendo solicitud para obtener productos por ids.");
        var products = await useCase.Execute(ids);
        logger.Log($"Productos obtenidos con éxito. Cantidad: {products.Count}");

        var apiResponse = new ApiResponse { Message = "Productos obtenidos con éxito", Data = products, Success = true };
        return Results.Ok(apiResponse);
    }
    catch (Exception ex)
    {
        logger.Log($"Error al obtener los productos: {ex.Message} (Inner: {ex.InnerException}) (Id's recibidos: {string.Join(", ", ids)})");
        return Results.BadRequest(new ApiResponse { Message = $"Error al obtener los productos: {ex.Message}", Error = ex.Message, Success = false });
    }
})
.WithName("GetProductosByIdsCPE")
.WithDescription("Gets the list of products by ids, but also filtering the CIDVALORCLASIFICACIO6 field, it ignores the 0 value i this field")
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
