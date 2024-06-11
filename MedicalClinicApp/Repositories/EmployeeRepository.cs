using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Models;
using MedicalClinicApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class EmployeeRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public EmployeeRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetPostList()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAll()
        {
            return await _context.Employees
                .Include(x => x.User).Where(x => x.User.RoleId == 2)
                .Include(x => x.Post)
                .Select(x => new EmployeeViewModel
                {
                    EmployeeId = x.EmployeeId,
                    UserId = x.UserId,
                    Email = x.User.Email,
                    PostId = x.PostId,
                    PostName = x.Post.Name,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    LastName = x.LastName,
                    OnboardingDate = x.OnboardingDate
                })
                .ToListAsync();
        }

        public async Task<EmployeeViewModel> Get(int employeeId)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (user == null)
            {
                return null;
            }

            return new EmployeeViewModel
            {
                EmployeeId = user.EmployeeId,
                UserId = user.UserId,
                Email = _context.Users.First(x => x.UserId == user.UserId).Email,
                PostId = user.PostId,
                PostName = _context.Posts.First(x => x.PostId == user.PostId).Name,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                OnboardingDate = user.OnboardingDate
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

        public async Task Create(EmployeeViewModel model)
        {
            AuthService.CreatePasswordHash(model.Password, out byte[] hash, out byte[] salt);
            var user = new User
            {
                Email = model.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = 2
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var employee = new Employee
            {
                UserId = user.UserId,
                PostId = model.PostId,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                LastName = model.LastName,
                OnboardingDate = model.OnboardingDate,
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(EmployeeViewModel model)
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
            var currentEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);
            if (currentEmployee == null)
            {
                return;
            }

            currentEmployee.PostId = model.PostId;
            currentEmployee.FirstName = model.FirstName;
            currentEmployee.SecondName = model.SecondName;
            currentEmployee.LastName = model.LastName;
            currentEmployee.OnboardingDate = model.OnboardingDate;
            _context.Employees.Update(currentEmployee);
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
