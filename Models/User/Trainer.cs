using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialWorkouts.ApplicationDb.Models
{
    public class TrainerEntity : UserEntity
    {
        // public int UserId {get; set;}
        [DefaultValue("BSc in Kinesiology from the University of Calgary")]
        public string? Credentials {get; set;}
    }
    
}