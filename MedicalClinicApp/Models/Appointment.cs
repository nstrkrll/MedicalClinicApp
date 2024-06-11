using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class Appointment
    {
        [Key]
        public int? AppointmentId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PatientId { get; set; }
        public int? AppointmentStatusId { get; set; }
        public string RoomNumber {  get; set; }
        public DateTime AppointmentDate { get; set; }
        public Employee Employee { get; set; }
        public Patient Patient { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public MedicalHistory MedicalHistory { get; set; }
    }
}
