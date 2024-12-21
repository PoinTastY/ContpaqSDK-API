using System.Text.Json;
using Application.UseCases.Postgres;
using Application.UseCases.Postgres.Movimientos;
using Application.UseCases.SDK;
using Application.UseCases.SDK.Documentos;
using Application.UseCases.SDK.Movimientos;
using Application.UseCases.SQL.ClienteProveedor;
using Application.UseCases.SQL.Documentos;
using Application.UseCases.SQL.Movimientos;
using Application.UseCases.SQL.Productos;
using Domain.Interfaces;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Repos.PostgreRepo;
using Domain.SDK_Comercial;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Postgres;
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
            var directoryPath = Path.GetDirectoryName(logFilePath) ?? throw new Exception("Directory path is null");

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
            builder.Services.AddDbContext<PostgresCPEContext>(options =>
            {
                options.UseNpgsql(sdkSettings.PostgresConnectionString,
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null));
            });
            builder.Services.AddSingleton<SDKRepo>();
            builder.Services.AddSingleton<ISDKRepo>(sp => sp.GetRequiredService<SDKRepo>());
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<IClienteProveedorRepo, ClienteProveedorRepo>();
            builder.Services.AddScoped<IDocumentRepo, DocumentRepo>();

            //for postgres
            builder.Services.AddScoped<IDocumentoRepo, DocumentoRepo>();
            builder.Services.AddScoped<Domain.Interfaces.Repos.PostgreRepo.IMovimientoRepo, Infrastructure.Repositories.Postgres.MovimientoRepo>();


            //UseCases
            #region SDK Services

            builder.Services.AddTransient<AddDocumentAndMovementsSDKUseCase>();
            builder.Services.AddTransient<PatchMovimientoUnidadesByIdUseCase>();
            builder.Services.AddTransient<TestSDKUseCase>();
            builder.Services.AddTransient<SetDocumentoImpresoSDKUseCase>();
            builder.Services.AddTransient<AddDocumentWithMovementSDKUseCase>();

            #endregion

            #region SQL Services

            #region Productos

            builder.Services.AddTransient<SearchProductosByNameSQLUseCase>();
            builder.Services.AddTransient<GetProductosByIdsSQLUseCase>();

            #endregion

            #region ClienteProveedor

            builder.Services.AddTransient<SearchClienteProveedorByNameSQLUseCase>();

            #endregion

            #region Documentos

            builder.Services.AddTransient<GetDocumentosByClienteAndDateSQLUseCase>();

            #endregion

            #endregion

            #region Postgres Services

            builder.Services.AddTransient<AddDocumentAndMovementsPostgresUseCase>();
            builder.Services.AddTransient<GetDocumentosPendientesUseCase>();
            builder.Services.AddTransient<UpdateDocumentoPendientePostgresUseCase>();

            #region Movimientos

            builder.Services.AddTransient<GetMovimientosByDocumentoIdPostgresUseCase>();
            builder.Services.AddTransient<UpdateMovimientosPostgresUseCase>();

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
                    return JsonSerializer.Deserialize<SDKSettings>(json) ?? throw new Exception("Json SDKSettings invalido");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
