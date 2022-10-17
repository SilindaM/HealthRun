using System.ComponentModel.DataAnnotations;

namespace HealthRun.Models
{
    public class ClubAttendees
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid ClubId { get; set; }
        public Club Club;
        public bool isHost { get; set; }
    }
}
