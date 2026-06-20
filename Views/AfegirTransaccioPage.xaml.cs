using CorIncrescendo.ViewModels;

namespace CorIncrescendo.Views;

public partial class AfegirTransaccioPage : ContentPage
{
    public AfegirTransaccioPage(AfegirTransaccioViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}