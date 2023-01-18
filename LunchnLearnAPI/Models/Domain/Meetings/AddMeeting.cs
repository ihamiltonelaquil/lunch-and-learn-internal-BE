namespace LunchnLearnAPI.Models.Domain
{

    public class AddMeeting
    {
        public string AuthID { get; set; }
        public string CreatorName { get; set; }
        public DateTime? MeetingStart { get; set; }
        public DateTime? MeetingEnd { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string? LinkToSlides { get; set; }
        public string? TeamsLink { get; set; }
    }
}
