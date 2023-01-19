using System.ComponentModel.DataAnnotations;

namespace LunchnLearnAPI.Models.Domain
{
    public class Users
    {
        [Key]
        public string AuthID { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
