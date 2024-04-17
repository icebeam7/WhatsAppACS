using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

using WhatsAppACS.Models;
using WhatsAppACS.Services;
using WhatsAppACS.Helpers;

namespace WhatsAppACS.ViewModels
{
    public partial class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<WaMessage> WhatsAppMessages { get; } = new();

        [ObservableProperty]
        string phoneNumber;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string message;

        [ObservableProperty]
        string mediaPath;

        List<string> recipients;
        Dictionary<string, string> arguments;
        string template;
        string templateLanguage;

        FileResult selectedFile;
        IFilePicker filePicker;
        IFileSystem fileSystem;

        IStorageService storageService;
        IWhatsAppService whatsAppService;
        Random generator;
        int maxNumber = 99999;

        public ChatViewModel(IStorageService storageService, 
            IWhatsAppService whatsAppService, 
            IFilePicker filePicker,
            IFileSystem fileSystem)
        {
            this.storageService = storageService;
            this.whatsAppService = whatsAppService;
            this.filePicker = filePicker;
            this.fileSystem = fileSystem;

            template = Constants.WhatsAppTemplate;
            templateLanguage = Constants.WhatsAppTemplateLanguage;
            generator = new Random();
        }

        [RelayCommand]
        async Task StartConversation()
        {
            if (IsBusy)
                return;

            try
            {
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    IsBusy = true;
                    
                    var shipmentNumber = generator.Next(maxNumber + 1).ToString("D6");
                    recipients = [PhoneNumber];

                    arguments = new() 
                    { 
                        ["value1"] = Name,
                        ["value2"] = shipmentNumber
                    };

                    var waMessage = new WaMessage()
                    {
                        To = recipients.First(),
                        Id = $"C{WhatsAppMessages.Count}",
                        Text = Message,
                        EventTime = DateTime.Now
                    };

                    WhatsAppMessages.Clear();
                    WhatsAppMessages.Add(waMessage);

                    await whatsAppService.StartConversation(
                        recipients, template, templateLanguage, arguments);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally 
            { 
                IsBusy = false; 
            }
        }

        [RelayCommand]
        async Task SelectFile()
        {
            try
            {
                selectedFile = await filePicker.PickAsync();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                if (recipients == null)
                    recipients = [PhoneNumber];

                var waMessage = new WaMessage()
                {
                    From = recipients.First(),
                    Id = $"S{WhatsAppMessages.Count}",
                    Text = Message,
                    EventTime = DateTime.Now
                };

                if (selectedFile != null)
                {
                    var blobUrl = await storageService.UploadBlob(selectedFile);
                    var blobUri = new Uri(blobUrl);
                    waMessage.Media = blobUrl;

                    await whatsAppService.SendMediaMessage(recipients, blobUri, Message);
                }
                else
                {
                    await whatsAppService.SendTextMessage(recipients, Message);
                }

                WhatsAppMessages.Add(waMessage);
                MediaPath = string.Empty;
                Message = string.Empty;
                selectedFile = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
