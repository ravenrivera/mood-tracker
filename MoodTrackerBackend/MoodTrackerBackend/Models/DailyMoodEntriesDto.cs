namespace MoodTrackerBackend.Models
{
    public class DailyMoodEntriesDto
    {
        public Guid Id { get; set; }
        public DateTime CreateOn { get; set; }
        public string CurrentMood { get; set; }
        public string Thoughts { get; set; }
        public List<ActivitiesDto> DailyActivity { get; set; }

    }
}
