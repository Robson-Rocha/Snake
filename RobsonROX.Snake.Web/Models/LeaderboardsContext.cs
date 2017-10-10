using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobsonROX.Snake.Web.Models
{
    public class LeaderboardsContext : DbContext
    {
        private static Lazy<LeaderboardsContext> _context = new Lazy<LeaderboardsContext>();
        public static LeaderboardsContext Db => _context.Value;

        public DbSet<Leaderboard> Leaderboards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Leaderboards");

        }
    }
}
