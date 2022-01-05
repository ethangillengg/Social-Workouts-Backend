using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SocialWorkouts.Services;

namespace SocialWorkouts.ApplicationDb.Models
{
    [Microsoft.EntityFrameworkCore.Index(nameof(UserEntity.Username), IsUnique = true)]
    public class UserEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), DefaultValue(0)]
        public int? Id { get => id; private set => id = value; }
        [JsonIgnore]
        public int? id;
        [Required]
        [DefaultValue("AmCam")]
        public string? Username { get; set; }
        [Required]
        [DefaultValue("Amanda")]
        public string? FName { get; set; }

        [Required]
        [DefaultValue("Cameron")]
        public string? LName { get; set; }

        [DefaultValue("amanda.cameron@mail.com")]
        public string? Email { get; set; }

        [DefaultValue("pw")]

        [NotMapped]
        [JsonPropertyName("password")]
        public string? JsonSetPassword { set => hashedPassword = PasswordHasher.Hash(value); } //Make sure to hash the passwords so that they cannot be gotten easily

        [JsonIgnore]
        public string? Password { get => hashedPassword; set => hashedPassword = value; }
        public bool Login(string password)
        {
            return PasswordHasher.CompHash(password, this.hashedPassword);
        }
        private string? hashedPassword;
        [NotMapped]
        [JsonPropertyName("type")]
        [DefaultValue("StandardUserEntity")]
        public string? UserType { get => this.GetType().Name.Replace("Entity", ""); }
        [DefaultValue("Hi my name is Amanda! I like working out!!")]
        public string? Bio { get; set; }
    }
}