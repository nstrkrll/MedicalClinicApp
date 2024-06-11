using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int? DoctorScheduleId { get; set; }
        public int EmployeeId { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public byte AppointmentsPerShift { get; set; }
        public string RoomNumber {  get; set; }
        public Employee Employee { get; set; }
    }
}