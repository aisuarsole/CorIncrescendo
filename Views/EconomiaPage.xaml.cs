using CorIncrescendo.ViewModels;

namespace CorIncrescendo.Views
{
    public partial class EconomiaPage : ContentPage
    {
        public EconomiaPage(EconomiaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is EconomiaViewModel vm)
                vm.OnAppearing();
        }
    }
}

