using System.ComponentModel.DataAnnotations;

namespace HealthRun.Models
{
    public class UserFollowing
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ObserverId { get; set; }
        public AppUser Observer { get; set; }
        public string TargetId { get; set; }
        public AppUser Target { get; set; }
    }
}
