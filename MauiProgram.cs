using CorIncrescendo.Services;
using CorIncrescendo.ViewModels;
using CorIncrescendo.Views;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Core;
using Plugin.Firebase.Firestore;
using Microsoft.Maui.LifecycleEvents;


#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif

namespace CorIncrescendo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterFirebaseServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Serveis
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<EconomiaService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<EconomiaViewModel>();
        builder.Services.AddTransient<AfegirTransaccioViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<EconomiaPage>();
        builder.Services.AddTransient<AfegirTransaccioPage>();

        return builder.Build();
    }

    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android.OnCreate((activity, _) =>
            {
                CrossFirebase.Initialize(activity, new CrossFirebaseSettings(
                    isAuthEnabled: true,
                    isFirestoreEnabled: true));
            }));
#endif
        });

        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
        builder.Services.AddSingleton(_ => CrossFirebaseFirestore.Current);

        return builder;
    }
}