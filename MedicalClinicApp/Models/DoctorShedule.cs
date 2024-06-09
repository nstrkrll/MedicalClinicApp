namespace MedicalClinicApp.Models
{
    public class DoctorShedule
    {
        public int SheduleId { get; set; }
        public int EmployeeId { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public byte AppointmentsPerShift { get; set; }
        public string RoomNumber {  get; set; }
    }
}
