using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class Employee
    {
        [Key]
        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime OnboardingDate { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public ICollection<DoctorSchedule> DoctorSchedules { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
