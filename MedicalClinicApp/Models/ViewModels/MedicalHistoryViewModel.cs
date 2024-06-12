namespace MedicalClinicApp.Models.ViewModels
{
    public class MedicalHistoryViewModel
    {
        public int? MedicalHistoryId { get; set; }
        public int? AppointmentId { get; set; }
        public string PatientComplaints { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
    }
}