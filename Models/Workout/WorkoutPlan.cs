using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SocialWorkouts.ApplicationDb.Models
{
    // [Microsoft.EntityFrameworkCore.Index(nameof(WorkoutPlanEntity.Name), IsUnique = true)]
    public class WorkoutPlanEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), DefaultValue(0)]
        public int? Id { get; private set; }
        [Required]
        [DefaultValue("Thunder Thighs 9000")]
        public string? Name { get; init; }
        public List<WorkoutEntity>? Workouts { get; set; }
        [ForeignKey("Trainer")]
        public int? TrainerId { get; set; }
        [JsonIgnore]
        public TrainerEntity? Trainer { get; set; }
        [DefaultValue(99.99)]
        public double Cost { get; set; }
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreated { get; private set; }
        [JsonIgnore]
        public bool Listed { get; set; } = true;
        [DefaultValue("This plan will help you get swole!")]
        public string? Description { get; set; }
    }
}