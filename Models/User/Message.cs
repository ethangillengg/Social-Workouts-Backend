using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace SocialWorkouts.ApplicationDb.Models
{
    public class MessageEntity
    {
        public MessageEntity() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [DefaultValue(1)]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        [JsonIgnore]
        public UserEntity? Sender { get; set; }
        [DefaultValue(2)]
        public int ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        [JsonIgnore]
        public UserEntity? Receiver { get; set; }
        [DefaultValue("Hello #2!! How is your day?")]
        public string? Content { get; set; }
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateSent { get; private set; }
    }
}