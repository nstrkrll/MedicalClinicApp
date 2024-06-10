using MedicalClinicApp.Models;
using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Services;

namespace MedicalClinicApp.Repositories
{
    public class AccountRepository
    {
        private readonly MedicalClinicDBContext _context;

        public AccountRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public bool CheckAuth(UserViewModel credentials)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == credentials.Login);
            if (user != null)
            {
                if (AuthService.VerifyPassword(credentials.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckUserExists(UserViewModel credentials)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == credentials.Login);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        public bool Register(UserViewModel credentials)
        {
            try
            {
                if (CheckUserExists(credentials))
                {
                    return false;
                }

                AuthService.CreatePasswordHash(credentials.Password, out byte[] hash, out byte[] salt);
                var newUser = new User
                {
                    UserId = null,
                    Email = credentials.Login,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    RoleId = 1
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}