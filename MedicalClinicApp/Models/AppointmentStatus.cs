using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class AppointmentStatus
    {
        [Key]
        public int? AppointmentStatusId { get; set; }
        public string StatusName { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
