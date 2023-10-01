using Microsoft.Extensions.Logging;

namespace DNDHelper
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
                    fonts.AddFont("QuartzoBold-W9lv.ttf", "Quartzo");
                    fonts.AddFont("MouldyCheeseRegular-WyMWG.ttf", "MouldyCheese");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

    }
}