namespace WhatsAppACS.Models
{
    public class WaMessage
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public string Media { get; set; }
        public string MimeType { get; set; }
        public DateTime EventTime { get; set; }
    }
}
