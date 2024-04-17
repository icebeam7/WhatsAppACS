using Azure.Communication.Messages;
using Azure.Communication.Messages.Models.Channels;

using WhatsAppACS.Helpers;

namespace WhatsAppACS.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        NotificationMessagesClient nmClient;
        Guid channelId;

        public WhatsAppService()
        {
            nmClient = new NotificationMessagesClient(Constants.AcsConnectionString);
            channelId = new Guid(Constants.AcsChannelId);
        }

        public async Task StartConversation(List<string> recipients, string template, 
            string templateLanguage, Dictionary<string, string> arguments)
        {
            var messageTemplate = new MessageTemplate(template, templateLanguage);
            var wamtBindings = new WhatsAppMessageTemplateBindings();

            foreach (var entry in arguments)
            {
                var mtText = new MessageTemplateText(entry.Key, entry.Value);
                messageTemplate.Values.Add(mtText);
                wamtBindings.Body.Add(new(mtText.Name));
            }

            messageTemplate.Bindings = wamtBindings;

            var tempNContent = 
                new TemplateNotificationContent(
                    channelId, recipients, messageTemplate);
            var response = await nmClient.SendAsync(tempNContent);
        }

        public async Task SendTextMessage(List<string> recipients, string message)
        {
            var textNContent = 
                new TextNotificationContent(
                    channelId, recipients, message);

            var response = await nmClient.SendAsync(textNContent);
        }

        public async Task SendMediaMessage(List<string> recipients, Uri mediaUri, string message = "")
        {
            var mediaNContent = 
                new MediaNotificationContent(
                    channelId, recipients, mediaUri);

            if (!string.IsNullOrWhiteSpace(message))
                mediaNContent.Content = message;

            var response = await nmClient.SendAsync(mediaNContent);
        }

        public async Task ReadMessage()
        {

        }
    }
}
