using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialWorkouts.ApplicationDb.Models
{

    [Owned]
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        [DefaultValue("Squats")]
        public string? Name { get; set; }
        [DefaultValue("6")]
        public int? Reps { get; set; }
        [DefaultValue("4")]
        public int? Sets { get; set; }
        [DefaultValue("50kg")]
        public string? Weight { get; set; }

    }
}