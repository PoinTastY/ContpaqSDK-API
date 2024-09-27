using Application.Services;
using Application.ViewModels;
using Microsoft.Extensions.Logging;
using Y_Report.Views;

namespace Y_Report
{
    public static class MauiProgram
    {
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

            return builder.Build();
        }

        private static void ConfigureServices(MauiAppBuilder builder)
        {
            var url = "http://26.116.39.19:4204";
            var apiService = new ApiService(url);
            builder.Services.AddSingleton(apiService);
            builder.Services.AddTransient<VMDocumentByConceptoSerieAndFolio>();
            builder.Services.AddTransient<SearchDocumentByCodesView>();

        }
    }
}
