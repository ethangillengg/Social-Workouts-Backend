using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SocialWorkouts.ApplicationDb.Models
{
    public class WorkoutEntity
    {
        [DefaultValue(1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // [JsonIgnore]
        public int Id { get; private set; }
        public ICollection<Exercise>? Exercises { get; set; }
        // [Timestamp]
        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        // public DateTime? Date { get; private set; }
    }
}