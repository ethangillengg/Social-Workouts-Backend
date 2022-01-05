using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialWorkouts.ApplicationDb.Models
{

    public class StandardUserEntity : UserEntity
    {
        // public int Id {get; set;}
        [DefaultValue("Not Selected")]
        public string? FitnessLevel { get; set; }
        // [DefaultValue(null)]
        public WorkoutEntity? NextWorkout { get; set; }
        // [DefaultValue(null)]
        // public SearchPreferencesEntity? SearchPreferences{get; set;}
        // public string? CurrentWorkoutPlanName{get; set;}
    }
}