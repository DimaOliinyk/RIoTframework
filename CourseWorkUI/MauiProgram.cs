using Microsoft.Extensions.Logging;

namespace CourseWorkUI
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
                    fonts.AddFont("Tomorrow-Thin.ttf", "MyMainFont");
                    fonts.AddFont("Tomorrow-Medium.ttf", "MyMainHeavyFont");
                    fonts.AddFont("Tomorrow-Regular.ttf", "MyMainMediumFont");
                    fonts.AddFont("Saira-Condensed-Light.ttf", "MySeconderyLightFont");
                    fonts.AddFont("Saira-Condensed-Regular.ttf", "MySeconderyMediumFont");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
