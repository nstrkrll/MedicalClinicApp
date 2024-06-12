using MedicalClinicApp.Models;
using MedicalClinicApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class AppointmentRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public AppointmentRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public async Task<int?> GetPatientIdByEmail(string email)
        {
            var user = await _context.Patients
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Email == email);
            if (user == null)
            {
                return -1;
            }

            return user.PatientId;
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAll()
        {
            return await _context.Appointments
                .Include(x => x.Employee)
                .ThenInclude(x => x.Post)
                .Include(x => x.Patient)
                .Include(x => x.AppointmentStatus)
                .Select(x => new AppointmentViewModel
                {
                    AppointmentId = x.AppointmentId,
                    EmployeeId = x.EmployeeId,
                    PatientId = x.PatientId,
                    AppointmentStatusId = x.AppointmentStatusId,
                    AppointmentStatusName = x.AppointmentStatus.StatusName,
                    PatientFullName = $"{x.Patient.LastName} {x.Patient.FirstName[0]}.{x.Patient.SecondName[0]}",
                    EmployeeFullName = $"{x.Employee.LastName} {x.Employee.FirstName[0]}.{x.Employee.SecondName[0]}",
                    EmployeePostName = x.Employee.Post.Name,
                    AppointmentDate = x.AppointmentDate,
                    RoomNumber = x.RoomNumber
                })
                .OrderBy(x => x.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetByPatient(string email)
        {
            var patient = await _context.Patients
                .Include(x => x.User)
                .Where(x => x.User.Email == email)
                .FirstOrDefaultAsync();
            if (patient == null)
            {
                return new List<AppointmentViewModel>();
            }

            return (await GetAll()).Where(x => x.PatientId == patient.PatientId);
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetByEmployee(string email)
        {
            var employee = await _context.Employees
                .Include(x => x.User)
                .Where(x => x.User.Email == email)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return new List<AppointmentViewModel>();
            }

            return (await GetAll()).Where(x => x.EmployeeId == employee.EmployeeId);
        }

        public async Task<IEnumerable<EmployeePreviewViewModel>> GetEmployeesList()
        {
            return await _context.Employees
                .Include(x => x.Post)
                .Select(x => new EmployeePreviewViewModel
                {
                    EmployeeId = x.EmployeeId,
                    PostName = x.Post.Name,
                    FirstName = x.FirstName,
                    SecondName = x.SecondName,
                    LastName = x.LastName
                })
                .OrderBy(x => x.PostName)
                .ToListAsync();
        }

        public async Task<IEnumerable<TimeSlotViewModel>> GetTimeSlots(int? employeeId, DateTime date)
        {
            byte dayOfWeek = (byte)date.DayOfWeek;
            var schedule = await _context.DoctorSchedules.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.DayOfWeek == dayOfWeek);
            if (schedule == null)
            {
                return new List<TimeSlotViewModel>();
            }

            if (date.Date == DateTime.Now.Date)
            {
                if (schedule.StartTime <= DateTime.Now.TimeOfDay)
                {
                    return new List<TimeSlotViewModel>();
                }
            }

            var timeShift = schedule.EndTime - schedule.StartTime;
            var hours = timeShift.Hours;
            var minutes = timeShift.Minutes;
            var averageMinutsPerAppoinment = (hours * 60 + minutes) / schedule.AppointmentsPerShift;
            var shift = new TimeSpan(averageMinutsPerAppoinment / 60, averageMinutsPerAppoinment % 60, 0);
            var slots = new List<TimeSlotViewModel>();
            var timeSlot = schedule.StartTime;
            while (timeSlot < schedule.EndTime)
            {
                slots.Add(new TimeSlotViewModel { DateTime = date.Add(timeSlot) });
                timeSlot += shift;
            }

            var appointments = await _context.Appointments.Where(x => x.EmployeeId == employeeId && x.AppointmentDate.Date == date.Date).ToListAsync();
            slots = slots.Where(x => !appointments.Where(y => y.AppointmentStatusId != 2).Any(y => y.AppointmentDate == x.DateTime)).ToList();
            return slots;
        }

        public async Task Create(TimeSlotSelectionViewModel model)
        {
            var appointments = await _context.DoctorSchedules.FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId && x.DayOfWeek == (byte)model.SelectedSlot.DayOfWeek);
            var appointment = new Appointment
            {
                EmployeeId = model.EmployeeId,
                PatientId = model.PatientId,
                AppointmentStatusId = 1,
                AppointmentDate = model.SelectedSlot,
                RoomNumber = appointments.RoomNumber
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == id);
            appointment.AppointmentStatusId = 2;
            _context.Appointments.Update(appointment);
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
