using System.ComponentModel.DataAnnotations;

namespace LunchnLearnAPI.Models.Domain
{
    public class LinkContainer
    {
        [Key]
        public Guid LinkID { get; set; }
        public string Link { get; set; }

        public string Name { get; set; }
        public Meeting Meeting { get; set; }
    }
}
