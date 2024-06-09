namespace MedicalClinicApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Email {  get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
