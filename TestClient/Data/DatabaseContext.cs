using Microsoft.EntityFrameworkCore;
using TestClient.Data.Model;

namespace TestClient.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<SettingQueryResult> SettingQueryResults { get; set; }
        public DbSet<WebService> WebService { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TestClientDB.db");
        }
    }
}
