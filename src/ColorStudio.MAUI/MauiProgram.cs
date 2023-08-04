using ColorStudio.MAUI.Services;
using ColorStudio.MAUI.ViewModels;
using Microsoft.Extensions.Logging;

namespace ColorStudio.MAUI
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
                })
                .Services
                .AddSingleton<ThemeColorExtractionService>()
                .AddSingleton(MediaPicker.Default)
                .AddSingleton<MainPageViewModel>()
                .AddSingleton<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif             
            return builder.Build();
        }
    }
}