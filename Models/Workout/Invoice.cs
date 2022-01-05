using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialWorkouts.ApplicationDb.Models
{
    public class InvoiceEntity
    {
        [Key]
        public int Id { get; set; }
        public double? Cost { get; set; }
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DatePurchased { get; private set; }

        [ForeignKey("UserEntity")]
        public int UserId { get; set; }
        [JsonIgnore]
        public UserEntity? User { get; set; }
        [ForeignKey("WorkoutPlanEntity")]
        [JsonIgnore]
        public WorkoutPlanEntity? WorkoutPlan { get; set; }
    }
}