using Microsoft.Extensions.Logging;
using Microcharts.Maui; // Para ChartView
using Microcharts;
using SkiaSharp.Views.Maui.Controls.Hosting; // Para Chart

namespace CoteAqui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-Bold.ttf", "pbold");
                    fonts.AddFont("Poppins-Medium.ttf", "pmedium");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
