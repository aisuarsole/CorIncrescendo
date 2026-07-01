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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is EconomiaViewModel vm)
                await vm.OnAppearing();
        }
    }
}

