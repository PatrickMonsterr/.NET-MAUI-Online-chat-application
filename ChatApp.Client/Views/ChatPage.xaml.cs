using ChatApp.Client.ViewModels;

namespace ChatApp.Client.Views;

public partial class ChatPage : ContentPage
{
    private readonly ChatViewModel _viewModel;

    public ChatPage()
    {
        InitializeComponent();
        _viewModel = new ChatViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await _viewModel.Disconnect();
    }
}
