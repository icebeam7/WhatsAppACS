namespace WhatsAppACS.Models
{
    public class WhatsAppResponse
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public string Subject { get; set; }
        public Data Data { get; set; }
        public string EventType { get; set; }
        public string DataVersion { get; set; }
        public string MetadataVersion { get; set; }
        public DateTime EventTime { get; set; }
    }

    public class Data
    {
        public string ChannelType { get; set; }
        public string Content { get; set; }
        public Media Media { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
    }

    public class Media
    {
        public string MimeType { get; set; }
        public string Id { get; set; }
    }
}
