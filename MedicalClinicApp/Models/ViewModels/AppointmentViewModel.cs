namespace MedicalClinicApp.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int? AppointmentId { get; set; }
        public int? EmployeeId { get; set; }
        public int? PatientId { get; set; }
        public int? AppointmentStatusId { get; set; }
        public string? AppointmentStatusName {  get; set; }
        public string? PatientFullName { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? EmployeePostName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string RoomNumber {  get; set; }
    }
}