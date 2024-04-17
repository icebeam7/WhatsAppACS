namespace WhatsAppACS.Services
{
    public interface IStorageService
    {
        Task<string> UploadBlob(FileResult file);
    }
}
