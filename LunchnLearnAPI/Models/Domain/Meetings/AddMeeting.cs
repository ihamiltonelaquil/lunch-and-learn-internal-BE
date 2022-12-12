namespace LunchnLearnAPI.Models.Domain
{
    public class AddMeeting
    {
        public string? CreatorName { get; set; }
        public DateTime? MeetingTime { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string? LinkToSlides { get; set; }
        public string? TeamsLink { get; set; }
    }
}
