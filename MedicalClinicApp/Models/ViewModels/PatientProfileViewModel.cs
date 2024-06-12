namespace MedicalClinicApp.Models.ViewModels
{
    public class PatientProfileViewModel
    {
        public int? PatientId { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
    }
}
