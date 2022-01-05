// using Microsoft.EntityFrameworkCore;
// using System.ComponentModel;
// using System.ComponentModel.DataAnnotations.Schema;
// using System.ComponentModel.DataAnnotations;
// using System.Text.Json.Serialization;

// namespace SocialWorkouts.ApplicationDb.Models
// {
//     public class SearchPreferencesEntity : UserEntity
//     {
//         [ForeignKey("Id")]
//         [JsonIgnore]
//         public StandardUserEntity? user { get; set; }
//         [DefaultValue("Strength")]
//         public string? WorkoutType { get; set; }
//         [DefaultValue("Active")]
//         public string? FitnessLevel { get; set; }
//         // [DefaultValue("Strength")]
//         public string? TrainerStatus { get; set; }
//         [DefaultValue("MTWF")]
//         public string? Availability { get; set; }
//         [DefaultValue("Female")]
//         public string? Gender { get; set; }
//     }
// }