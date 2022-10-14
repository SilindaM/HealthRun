namespace HealthRun.Models
{
    public class RaceAttendees
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid EventId { get; set; }
        public Race Race;
        public bool isHost { get; set; }
    }
}
