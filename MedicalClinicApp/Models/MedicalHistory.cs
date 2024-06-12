using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class MedicalHistory
    {
        [Key]
        public int? MedicalHistoryId { get; set; }
        public int? AppointmentId { get; set; }
        public string PatientComplaints {  get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public Appointment Appointment { get; set; }
    }
}