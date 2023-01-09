namespace LunchnLearnAPI.Models.Domain
{
    public class Attachment
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        public string BlobName { get; set; }
        public string? ContentType { get; set; }
        public string PublicURI { get; set; }
        public Meeting? Meeting { get; set; }
    }
}
