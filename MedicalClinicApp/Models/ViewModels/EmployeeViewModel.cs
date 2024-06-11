namespace MedicalClinicApp.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public string? PostName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime OnboardingDate { get; set; }
    }
}