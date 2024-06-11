using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public string Email {  get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual Role Role { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
