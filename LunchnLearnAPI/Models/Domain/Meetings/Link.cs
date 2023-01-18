namespace LunchnLearnAPI.Models.Domain
{
    public class Link
    {
        public Guid linkId { get; set; }
        public string linkName { get; set; }
        public string linkUrl { get; set; }
        public Meeting meeting { get; set; }
    }
}
