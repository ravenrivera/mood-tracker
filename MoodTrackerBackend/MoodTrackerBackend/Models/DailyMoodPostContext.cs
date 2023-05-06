using Microsoft.EntityFrameworkCore;

namespace MoodTrackerBackend.Models
{
    public class DailyMoodPostContext : DbContext
    {
        public DailyMoodPostContext(DbContextOptions<DailyMoodPostContext> options) : base(options) { }
        public DbSet<DailyMoodPostModel> DailyMoodEntries { get; set; } = null!;
        public DbSet<ActivitiesModel> ActivityEntries { get; set; } = null!;
    }
}
