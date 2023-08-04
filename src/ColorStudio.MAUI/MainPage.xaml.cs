using ColorStudio.MAUI.ViewModels;

namespace ColorStudio.MAUI;

public partial class MainPage : ContentPage
{
    public MainPageViewModel ViewModel { get; }

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        BindingContext = this;
    }
}