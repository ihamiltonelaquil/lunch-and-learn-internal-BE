namespace LunchnLearnAPI.Models.Domain
{
    public class Meeting
    {
        public string AuthID { get; set; }
        public Guid MeetingID { get; set; }
        public string CreatorName { get; set; }
        public DateTime? MeetingStart { get; set; }
        public DateTime? MeetingEnd { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
    }
}
