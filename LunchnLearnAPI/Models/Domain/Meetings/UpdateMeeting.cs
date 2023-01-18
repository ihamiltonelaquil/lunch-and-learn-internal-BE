namespace LunchnLearnAPI.Models.Domain.Meetings
{
    public class UpdateMeeting
    {
        public string? CreatorName { get; set; }
        public DateTime? MeetingStart { get; set; }
        public DateTime? MeetingEnd { get; set; }
        public string? Topic { get; set; }
        public string? Description { get; set; }
        public string? LinkToSlides { get; set; }
        public string? TeamsLink { get; set; }
    }
}
