using MedicalClinicApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class DoctorScheduleRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public DoctorScheduleRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorScheduleViewModel>> GetAll()
        {
            return await _context.DoctorSchedules
                .Include(x => x.Employee)
                .ThenInclude(x => x.Post)
                .Select(x => new DoctorScheduleViewModel
                {
                    DoctorScheduleId = x.DoctorScheduleId,
                    EmployeeId = x.EmployeeId,
                    PostName = x.Employee.Post.Name,
                    FirstName = x.Employee.FirstName,
                    SecondName = x.Employee.SecondName,
                    LastName = x.Employee.LastName,
                    DayOfWeek = x.DayOfWeek,
                    AppointmentsPerShift = x.AppointmentsPerShift,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    RoomNumber = x.RoomNumber
                })
                .ToListAsync();
        }

        public async Task<DoctorScheduleViewModel> Get(int doctorScheduleId)
        {
            var doctorSchedule = await _context.DoctorSchedules
                .Include(x => x.Employee)
                .ThenInclude(x => x.Post)
                .FirstOrDefaultAsync(x => x.DoctorScheduleId == doctorScheduleId);
            if (doctorSchedule == null)
            {
                return null;
            }

            return new DoctorScheduleViewModel
            {
                DoctorScheduleId = doctorSchedule.DoctorScheduleId,
                EmployeeId = doctorSchedule.EmployeeId,
                PostName = doctorSchedule.Employee.Post.Name,
                FirstName = doctorSchedule.Employee.FirstName,
                SecondName = doctorSchedule.Employee.SecondName,
                LastName = doctorSchedule.Employee.LastName,
                DayOfWeek = doctorSchedule.DayOfWeek,
                AppointmentsPerShift = doctorSchedule.AppointmentsPerShift,
                StartTime = doctorSchedule.StartTime,
                EndTime = doctorSchedule.EndTime,
                RoomNumber = doctorSchedule.RoomNumber
            };
        }

        public async Task Edit(DoctorScheduleViewModel model)
        {
            var currentSchedule = await _context.DoctorSchedules.FirstOrDefaultAsync(x => x.DoctorScheduleId == model.DoctorScheduleId);
            if (currentSchedule == null)
            {
                return;
            }

            currentSchedule.AppointmentsPerShift = model.AppointmentsPerShift;
            currentSchedule.RoomNumber = model.RoomNumber;
            currentSchedule.StartTime = model.StartTime;
            currentSchedule.EndTime = model.EndTime;
            _context.DoctorSchedules.Update(currentSchedule);
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
