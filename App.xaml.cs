using CorIncrescendo.Services;

namespace CorIncrescendo
{
    public partial class App : Application
    {
        public App(AuthService authService)
        {
            InitializeComponent();

            MainPage = new AppShell();

            // Si ja hi ha sessió activa, anar directe al menú principal
            if (authService.IsLoggedIn())
            {
                Dispatcher.Dispatch(async () =>
                    await Shell.Current.GoToAsync("//MainPage"));
            }
        }
    }
}
