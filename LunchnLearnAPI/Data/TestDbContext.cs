using Microsoft.EntityFrameworkCore;
using LunchnLearnAPI.Models.Domain;

namespace LunchnLearnAPI.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
