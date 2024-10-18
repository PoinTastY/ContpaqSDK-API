using System.Text.Json;
using Application.UseCases.SDK;
using Application.UseCases.SDK.Documentos;
using Application.UseCases.SDK.Movimientos;
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

namespace Pedidos_CPE.DI
{
    public static class DependencyServices
    {
        public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder)
        {

            //builder.Host.UseWindowsService();
            //builder.Services.AddWindowsService();//to use it as a windows service

            var sdkSettings = LoadSettings();

            var logFilePath = "C:\\Stare-y\\ContpaqSDK-API\\log.txt";
            string directoryPath = Path.GetDirectoryName(logFilePath);

            // Crea el directorio si no existe
            if (!Directory.Exists(directoryPath))
            {
                if (string.IsNullOrEmpty(directoryPath))
                {
                    throw new Exception("Directory path is empty");
                }
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

            #region Movimientos

            builder.Services.AddTransient<PatchMovimientoUnidadesByIdUseCase>();

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
            builder.Services.AddTransient<PatchUnidadesMovimientoByIdSQLUseCase>();

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

            return builder;
        }

        private static SDKSettings LoadSettings()
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
    }
}
