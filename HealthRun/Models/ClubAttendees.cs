namespace HealthRun.Models
{
    public class ClubAttendees
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid EventId { get; set; }
        public Club Club;
        public bool isHost { get; set; }
    }
}
