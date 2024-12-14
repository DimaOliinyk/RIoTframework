using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.LocalNotification;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace CourseWorkUI;

// Source for app events
// 

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()     // Plugin for Local Notifications
            .ConfigureLifecycleEvents(AppLifeCycle => 
            {
#if ANDROID
                AppLifeCycle.AddAndroid(android => android.OnPause((activity) => 
                {
                    if(AppState.IsRunning && !IDLEState.IsIdle)
                    {
                        Debug.WriteLine("RIOT: Entering Idle Mode");
                        AppState.TurnOff();
                        IDLEState.Change();
                        MainPage.IDLEData();
                    }
                }));
                AppLifeCycle.AddAndroid(android => android.OnRestart((activity) => 
                { 
                    if(!AppState.IsRunning && IDLEState.IsIdle)
                    {
                        Debug.WriteLine("RIOT: Entering Manual Mode");
                        AppState.Change();
                        IDLEState.TurnOff();
                        MainPage.ReadData();
                    }
                }));
#elif WINDOWS
                AppLifeCycle.AddWindows(windows => windows.OnVisibilityChanged((activity, args) => 
                { 
                    if(AppState.IsRunning && !IDLEState.IsIdle)
                    {
                        Debug.WriteLine("RIOT: Entering Idle Mode");
                        AppState.TurnOff();
                        IDLEState.Change();
                        MainPage.IDLEData();
                    }
                    else if(!AppState.IsRunning && IDLEState.IsIdle)
                    {
                        Debug.WriteLine("RIOT: Entering Manual Mode");
                        AppState.Change();
                        IDLEState.TurnOff();
                        MainPage.ReadData();
                    }
                }));
#else
# error AppLifecycle events are not set for this platform
#endif
            })
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
