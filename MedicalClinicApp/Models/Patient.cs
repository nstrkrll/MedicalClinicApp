﻿using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class Patient
    {
        [Key]
        public int? PatientId { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender {  get; set; }
        public virtual User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}