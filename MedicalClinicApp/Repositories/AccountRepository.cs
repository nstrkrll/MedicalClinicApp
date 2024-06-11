
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
                RoleId = 3
            };

            _context.Users.Add(newUser);
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