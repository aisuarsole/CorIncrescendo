namespace CorIncrescendo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Registrar rutas
            Routing.RegisterRoute("EconomiaPage", typeof(Views.EconomiaPage));
            Routing.RegisterRoute("AfegirTransaccioPage", typeof(Views.AfegirTransaccioPage));
        }
    }
}


