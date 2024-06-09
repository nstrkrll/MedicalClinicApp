namespace MedicalClinicApp.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int EmployeeId { get; set; }
        public int PatientId { get; set; }
        public int AppointmentStatusId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
