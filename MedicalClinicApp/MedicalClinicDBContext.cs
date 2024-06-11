using MedicalClinicApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace MedicalClinicApp
{
    public class MedicalClinicDBContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }

        public MedicalClinicDBContext(DbContextOptions<MedicalClinicDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasOne(x => x.User)
                .WithOne(x => x.Patient)
                .HasForeignKey<Patient>(x => x.UserId);

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.User)
                .WithOne(x => x.Employee)
                .HasForeignKey<Employee>(x => x.UserId);

            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Post)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<DoctorSchedule>()
                .HasIndex(x => new {x.EmployeeId, x.DayOfWeek })
                .IsUnique();

            modelBuilder.Entity<Appointment>()
                .HasOne(x => x.MedicalHistory)
                .WithOne(x => x.Appointment)
                .HasForeignKey<MedicalHistory>(x => x.AppointmentId);

            modelBuilder.Entity<Employee>()
                .ToTable(x => x.UseSqlOutputClause(false));
        }
    }
}