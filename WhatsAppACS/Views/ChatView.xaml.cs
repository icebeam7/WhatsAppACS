using WhatsAppACS.ViewModels;

namespace WhatsAppACS.Views;

public partial class ChatView : ContentPage
{
	public ChatView(ChatViewModel vm)
	{
		InitializeComponent();

        this.BindingContext = vm;
    }
}