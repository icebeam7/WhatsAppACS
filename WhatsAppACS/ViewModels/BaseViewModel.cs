using CommunityToolkit.Mvvm.ComponentModel;

namespace WhatsAppACS.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;

        [ObservableProperty]
        bool isBusy;
    }
}
