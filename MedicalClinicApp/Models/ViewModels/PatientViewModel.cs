namespace MedicalClinicApp.Models.ViewModels
{
    public class PatientViewModel
    {
        public int? PatientId {  get; set; }
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }

    }
}