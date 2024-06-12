using MedicalClinicApp.Models;
using Microsoft.Data.SqlClient;
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
        public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
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

        public async Task AddPatientAsync(int userId, string firstName, string secondName, string lastName, string phoneNumber, DateTime dateOfBirth, bool gender)
        {
            var userIdParam = new SqlParameter("@UserId", userId);
            var firstNameParam = new SqlParameter("@FirstName", firstName);
            var secondNameParam = new SqlParameter("@SecondName", secondName ?? (object)DBNull.Value);
            var lastNameParam = new SqlParameter("@LastName", lastName);
            var phoneNumberParam = new SqlParameter("@PhoneNumber", phoneNumber);
            var dateOfBirthParam = new SqlParameter("@DateOfBirth", dateOfBirth);
            var genderParam = new SqlParameter("@Gender", gender);

            await Database.ExecuteSqlRawAsync("EXEC AddPatient @UserId, @FirstName, @SecondName, @LastName, @PhoneNumber, @DateOfBirth, @Gender",
                userIdParam, firstNameParam, secondNameParam, lastNameParam, phoneNumberParam, dateOfBirthParam, genderParam);
        }

        public async Task AddEmployeeAsync(int userId, string firstName, string secondName, string lastName, DateTime onboardingDate, int postId)
        {
            var userIdParam = new SqlParameter("@UserId", userId);
            var firstNameParam = new SqlParameter("@FirstName", firstName);
            var secondNameParam = new SqlParameter("@SecondName", secondName ?? (object)DBNull.Value);
            var lastNameParam = new SqlParameter("@LastName", lastName);
            var onboardingDateParam = new SqlParameter("@OnboardingDate", onboardingDate);
            var postIdParam = new SqlParameter("@PostId", postId);

            await Database.ExecuteSqlRawAsync("EXEC AddEmployee @UserId, @FirstName, @SecondName, @LastName, @OnboardingDate, @PostId",
                userIdParam, firstNameParam, secondNameParam, lastNameParam, onboardingDateParam, postIdParam);
        }
    }
}