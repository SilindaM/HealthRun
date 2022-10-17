using System.ComponentModel.DataAnnotations;

namespace HealthRun.Models
{
    public class RaceAttendees
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid RaceId { get; set; }
        public Race Race;
        public bool isHost { get; set; }
    }
}
