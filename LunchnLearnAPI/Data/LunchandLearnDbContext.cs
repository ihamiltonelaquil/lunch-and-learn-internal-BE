using Microsoft.EntityFrameworkCore;
using LunchnLearnAPI.Models.Domain;

namespace LunchnLearnAPI.Data
{
    public class LunchandLearnDbContext : DbContext
    {
        public LunchandLearnDbContext(DbContextOptions<LunchandLearnDbContext> options)
            : base(options) { }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<LinkContainer> Links { get; set; }
        public DbSet<Users> Users { get; set; }

        [DbFunction(name: "SOUNDEX", IsBuiltIn = true)]
        public string FuzzySearch(string query)
        {
            throw new NotImplementedException();
        }
    }
}
