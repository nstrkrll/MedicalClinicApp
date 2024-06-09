using MedicalClinicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp
{
    public class MedicalClinicDBContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorShedule> DoctorShedules { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }

        public MedicalClinicDBContext(DbContextOptions<MedicalClinicDBContext> options) : base(options) { }
    }
}