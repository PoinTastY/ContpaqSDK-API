using Application.Services;
using Application.ViewModels;
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
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("http://192.168.1.75:4204");
            });

            builder.Services.AddTransient<VMDocumentByConceptoSerieAndFolio>();
            builder.Services.AddTransient<SearchDocumentByCodesView>();
            builder.Services.AddTransient<VMViewDocumentDetails>();
            builder.Services.AddTransient<ViewDocumentDetails>();
            builder.Services.AddTransient<ViewProducts>();
        }
    }
}
