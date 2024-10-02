using Application.ViewModels;
using Domain.Interfaces.Services.ApiServices.Documentos;
using Domain.Interfaces.Services.ApiServices.Movimientos;
using Domain.Interfaces.Services.ApiServices.Productos;
using Infrastructure.Services.API.Documentos;
using Infrastructure.Services.API.Movimientos;
using Infrastructure.Services.API.Productos;
using Microsoft.Extensions.Logging;
using Y_Report.Views;

namespace Y_Report
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            ConfigureServices(builder);

            ServiceProvider = builder.Services.BuildServiceProvider();

            return builder.Build();
        }

        private static void ConfigureServices(MauiAppBuilder builder)
        {
            builder.Services.AddHttpClient("CommonHttpClient", client =>
            {
                client.BaseAddress = new Uri("http://26.116.39.19:4204");
            });

            // Registrar los servicios e inyectar el HttpClient común
            builder.Services.AddTransient<IDocumentoService, DocumentoService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new DocumentoService(httpClientFactory.CreateClient("CommonHttpClient"));
            });

            builder.Services.AddTransient<IMovimientoService, MovimientoService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new MovimientoService(httpClientFactory.CreateClient("CommonHttpClient"));
            });

            builder.Services.AddTransient<IProductoService, ProductoService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return new ProductoService(httpClientFactory.CreateClient("CommonHttpClient"));
            });

            builder.Services.AddTransient<VMDocumentByConceptoSerieAndFolio>();
            builder.Services.AddTransient<VMViewDocumentDetails>();
            builder.Services.AddTransient<VMViewProducts>();
        }
    }
}
