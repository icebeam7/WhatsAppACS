namespace WhatsAppACS.Services
{
    public interface IWhatsAppService
    {
        Task StartConversation(List<string> recipients, string template, string templateLanguage, Dictionary<string, string> arguments);
        Task SendTextMessage(List<string> recipients, string message);
        Task SendMediaMessage(List<string> recipients, Uri mediaUri, string message = "");
        Task ReadMessage();
    }
}
