using MedicalClinicApp.Models;
using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class PatientRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public PatientRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientViewModel>> GetAll()
        {
            return await _context.Patients
                .Include(x => x.User).Where(x => x.User.RoleId == 3)
                .Select(x => new PatientViewModel
                {
                    PatientId = x.PatientId,
                    UserId = x.UserId,
                    Email = x.User.Email,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    DateOfBirth = x.DateOfBirth,
                    Gender = x.Gender
                })
                .ToListAsync();
        }

        public async Task<PatientViewModel> Get(int patientId)
        {
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == patientId);
            if (user == null)
            {
                return null;
            }

            return new PatientViewModel
            {
                PatientId = user.PatientId,
                UserId = user.UserId,
                Email = _context.Users.First(x => x.UserId == user.UserId).Email,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender
            };
        }

        public async Task<bool> CheckIfExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return false;
            }

            return true;
        }

        public async Task Create(PatientViewModel model)
        {
            AuthService.CreatePasswordHash(model.Password, out byte[] hash, out byte[] salt);
            var user = new User
            {
                Email = model.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = 3
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //var patient = new Patient
            //{
            //    UserId = user.UserId,
            //    FirstName = model.FirstName,
            //    SecondName = model.SecondName,
            //    LastName = model.LastName,
            //    PhoneNumber = model.PhoneNumber,
            //    DateOfBirth = model.DateOfBirth,
            //    Gender = model.Gender
            //};

            //_context.Patients.Add(patient);
            //await _context.SaveChangesAsync();
            await _context.AddPatientAsync((int)user.UserId, model.FirstName, model.SecondName, model.LastName, model.PhoneNumber, model.DateOfBirth, model.Gender);
        }

        public async Task Edit(PatientViewModel model)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.UserId == model.UserId);
            if (currentUser == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                AuthService.CreatePasswordHash(model.Password, out byte[] hash, out byte[] salt);
                currentUser.PasswordHash = hash;
                currentUser.PasswordSalt = salt;
            }

            currentUser.Email = model.Email;
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
            var currentPatient = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == model.PatientId);
            if (currentPatient == null)
            {
                return;
            }

            currentPatient.FirstName = model.FirstName;
            currentPatient.SecondName = model.SecondName;
            currentPatient.LastName = model.LastName;
            currentPatient.PhoneNumber = model.PhoneNumber;
            currentPatient.DateOfBirth = model.DateOfBirth;
            currentPatient.Gender = model.Gender;
            _context.Patients.Update(currentPatient);
            await _context.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}