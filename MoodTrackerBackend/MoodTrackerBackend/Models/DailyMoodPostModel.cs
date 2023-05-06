namespace MoodTrackerBackend.Models
{
    public class DailyMoodPostModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateOn { get; set; }
        public string Thoughts { get; set; }
        public Mood CurrentMood { get; set; }
    }
    public enum Mood
    {
        great,
        good,
        okay,
        bad,
        awful
    }
}
