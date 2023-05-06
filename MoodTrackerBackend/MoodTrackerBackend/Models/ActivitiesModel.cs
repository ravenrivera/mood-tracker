namespace MoodTrackerBackend.Models
{
    public class ActivitiesModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PostId { get; set; }
        public Activities DailyActivity { get; set; }
    }

    public enum Activities
    {
        study,
        exercise,
        goout,
        read,
        watchamovie,
        hobby
    }
}
