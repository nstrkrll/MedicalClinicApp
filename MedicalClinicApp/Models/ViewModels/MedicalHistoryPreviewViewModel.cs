namespace MedicalClinicApp.Models.ViewModels
{
    public class MedicalHistoryPreviewViewModel
    {
        public int? MedicalHistoryId { get; set; }
        public int? AppointmentId { get; set; }
        public int? PatientId { get; set; }
        public int? EmployeeId { get; set; }
        public string PatientComplaints { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string PatientFullName { get; set; }
        public string EmployeeFullName { get; set; }
        public string EmployeePostName { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
