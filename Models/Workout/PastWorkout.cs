using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SocialWorkouts.ApplicationDb.Models
{
    public class PastWorkoutRelation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public int Id { get; private set; }
        [ForeignKey("UserEntity")]
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public UserEntity? User { get; set; }
        [ForeignKey("WorkoutEntity")]
        [JsonIgnore]
        public int WorkoutId { get; set; }
        public WorkoutEntity? Workout { get; set; }
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateRecorded { get; private set; }

    }
}