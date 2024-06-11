namespace MedicalClinicApp.Models.ViewModels
{
    public class TimeSlotSelectionViewModel
    {
        public int? EmployeeId { get; set; }
        public int? PatientId { get; set; }
        public DateTime SelectedSlot { get; set; }
    }
}