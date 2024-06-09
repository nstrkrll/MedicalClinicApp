namespace MedicalClinicApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string FirstName { get; set; }
        public string SecondName {  get; set; }
        public string LastName { get; set; }
        public DateTime OnboardingDate { get; set; }
    }
}
