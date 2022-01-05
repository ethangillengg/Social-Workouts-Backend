using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SocialWorkouts.ApplicationDb.Models
{
    public class FriendRelation
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserEntity")]
        [DefaultValue(1)]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }

        [ForeignKey("UserEntity")]
        public int FriendId { get; set; }
        public UserEntity? Friend { get; set; }
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateFriended { get; private set; }
        // public DateTime DateFriended { get; set; }

    }
}