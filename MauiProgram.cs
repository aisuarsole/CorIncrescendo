using CorIncrescendo.Services;
using CorIncrescendo.ViewModels;
using CorIncrescendo.Views;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Firestore;
using Plugin.Firebase.Core;

namespace CorIncrescendo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseFirebase()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Firebase
        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
        builder.Services.AddSingleton(_ => CrossFirebaseFirestore.Current);

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
}