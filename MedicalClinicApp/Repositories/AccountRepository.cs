
using MedicalClinicApp.Models;
using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class AccountRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public AccountRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public async Task<User?> Get(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == login);
        }

        public async Task<bool> CheckAuth(UserViewModel credentials)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == credentials.Login);
            if (user != null)
            {
                if (AuthService.VerifyPassword(credentials.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task Register(UserViewModel credentials)
        {
            AuthService.CreatePasswordHash(credentials.Password, out byte[] hash, out byte[] salt);
            var newUser = new User
            {
                UserId = null,
                Email = credentials.Login,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = 4
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<PatientProfileViewModel> GetPatientProfile(string email)
        {
            var currentUser = _context.Users.First(x => x.Email == email);
            var currentPatient = await _context.Patients.FirstOrDefaultAsync(x => x.UserId == currentUser.UserId);
            if (currentPatient == null)
            {
                return new PatientProfileViewModel();
            }

            return new PatientProfileViewModel
            {
                PatientId = currentPatient.PatientId,
                FirstName = currentPatient.FirstName,
                SecondName = currentPatient.SecondName,
                LastName = currentPatient.LastName,
                PhoneNumber = currentPatient.PhoneNumber,
                DateOfBirth = currentPatient.DateOfBirth,
                Gender = currentPatient.Gender
            };
        }

        public async Task<EmployeePreviewViewModel> GetEmployeeProfile(string email)
        {
            var currentUser = _context.Users.First(x => x.Email == email);
            var currentEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.UserId == currentUser.UserId);
            if (currentEmployee == null)
            {
                return new EmployeePreviewViewModel();
            }

            return new EmployeePreviewViewModel
            {
                EmployeeId = currentEmployee.EmployeeId,
                FirstName = currentEmployee.FirstName,
                SecondName = currentEmployee.SecondName,
                LastName = currentEmployee.LastName,
            };
        }

        public async Task EditPatientProfile(PatientProfileViewModel model, string email)
        {
            var currentPatient = await _context.Patients.FirstOrDefaultAsync(x => x.PatientId == model.PatientId);
            if (currentPatient != null)
            {
                currentPatient.LastName = model.LastName;
                currentPatient.FirstName = model.FirstName;
                currentPatient.SecondName = model.SecondName;
                currentPatient.DateOfBirth = model.DateOfBirth;
                currentPatient.PhoneNumber = model.PhoneNumber;
                _context.Update(currentPatient);
                await _context.SaveChangesAsync();
                return;
            }

            var currentUser = _context.Users.First(x => x.Email == email);
            await _context.AddPatientAsync((int)currentUser.UserId, model.FirstName, model.SecondName, model.LastName, model.PhoneNumber, model.DateOfBirth, model.Gender);
            currentUser.RoleId = 3;
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
        }

        public async Task EditEmployeeProfile(EmployeePreviewViewModel model)
        {
            var currentEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);
            currentEmployee.LastName = model.LastName;
            currentEmployee.FirstName = model.FirstName;
            currentEmployee.SecondName = model.SecondName;
            _context.Update(currentEmployee);
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