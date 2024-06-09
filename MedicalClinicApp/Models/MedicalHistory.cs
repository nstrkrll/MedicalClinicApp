namespace MedicalClinicApp.Models
{
    public class MedicalHistory
    {
        public int HistoryId { get; set; }
        public int AppointmentId { get; set; }
        public string PatientComplaints {  get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}