using Microsoft.Extensions.Logging;
using WhatsAppACS.Services;
using WhatsAppACS.ViewModels;
using WhatsAppACS.Views;

namespace WhatsAppACS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IStorageService, StorageService>()
                .AddSingleton<IWhatsAppService, WhatsAppService>()
                .AddSingleton<IFileSystem>(FileSystem.Current)
                .AddSingleton<IFilePicker>(FilePicker.Default);

            builder.Services.AddScoped<ChatViewModel>();
            builder.Services.AddScoped<ChatView>();

            return builder.Build();
        }
    }
}
