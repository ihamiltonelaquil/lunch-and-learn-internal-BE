namespace LunchnLearnAPI.Models.Domain
{
    public class Meeting
    {
        public Guid MeetingID { get; set; }
        public string? CreatorName { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string? LinkToSlides {get; set;}
        public string? TeamsLink { get; set; }
    }
}
