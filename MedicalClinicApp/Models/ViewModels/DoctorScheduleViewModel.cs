namespace MedicalClinicApp.Models.ViewModels
{
    public class DoctorScheduleViewModel
    {
        public int? DoctorScheduleId { get; set; }
        public int? EmployeeId { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public byte AppointmentsPerShift { get; set; }
        public string RoomNumber { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? PostName { get; set; }
    }
}