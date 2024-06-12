using MedicalClinicApp.Models;
using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Services;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class MedicalHistoryRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public MedicalHistoryRepository(MedicalClinicDBContext context)
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
                return null;
            }

            return user.PatientId;
        }

        public async Task<IEnumerable<MedicalHistoryPreviewViewModel>> GetAll()
        {
            return await _context.MedicalHistory
                .Include(x => x.Appointment)
                .ThenInclude(x => x.Employee)
                .ThenInclude(x => x.Post)
                .Include(x => x.Appointment)
                .ThenInclude(x => x.Patient)
                .Select(x => new MedicalHistoryPreviewViewModel
                {
                    MedicalHistoryId = x.MedicalHistoryId,
                    AppointmentId = x.AppointmentId,
                    PatientId = x.Appointment.PatientId,
                    EmployeeId = x.Appointment.EmployeeId,
                    PatientComplaints = x.PatientComplaints,
                    Diagnosis = x.Diagnosis,
                    Treatment = x.Treatment,
                    PatientFullName = $"{x.Appointment.Patient.LastName} {x.Appointment.Patient.FirstName[0]}.{x.Appointment.Patient.SecondName[0]}",
                    EmployeeFullName = $"{x.Appointment.Employee.LastName} {x.Appointment.Employee.FirstName[0]}.{x.Appointment.Employee.SecondName[0]}",
                    EmployeePostName = x.Appointment.Employee.Post.Name,
                    AppointmentDate = x.Appointment.AppointmentDate,
                })
                .OrderBy(x => x.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalHistoryPreviewViewModel>> GetByPatient(string email)
        {
            var patient = await _context.Patients
                .Include(x => x.User)
                .Where(x => x.User.Email == email)
                .FirstOrDefaultAsync();
            if (patient == null)
            {
                return null;
            }

            return (await GetAll()).Where(x => x.PatientId == patient.PatientId);
        }

        public async Task<IEnumerable<MedicalHistoryPreviewViewModel>> GetByEmployee(string email)
        {
            var employee = await _context.Employees
                .Include(x => x.User)
                .Where(x => x.User.Email == email)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return null;
            }

            return (await GetAll()).Where(x => x.EmployeeId == employee.EmployeeId);
        }

        public async Task<MedicalHistoryViewModel> Get(int id)
        {
            var medicalHistory = await _context.MedicalHistory.FirstOrDefaultAsync(x => x.MedicalHistoryId == id);
            if (medicalHistory == null)
            {
                return null;
            }

            return new MedicalHistoryViewModel
            {
                MedicalHistoryId = medicalHistory.MedicalHistoryId,
                AppointmentId = medicalHistory.AppointmentId,
                PatientComplaints = medicalHistory.PatientComplaints,
                Diagnosis = medicalHistory.Diagnosis,
                Treatment = medicalHistory.Treatment
            };
        }

        public async Task<IEnumerable<AppointmentViewModel>> GetAppointmentsByEmployee(string email)
        {
            var employee = await _context.Employees
                .Include(x => x.User)
                .Where(x => x.User.Email == email)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return null;
            }

            return await _context.Appointments
                .Include(x => x.Employee)
                .ThenInclude(x => x.Post)
                .Include(x => x.Patient)
                .Include(x => x.AppointmentStatus)
                .Where(x => x.EmployeeId == employee.EmployeeId)
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

        public async Task Create(MedicalHistoryViewModel model)
        {
            var medicalHistory = new MedicalHistory
            {
                AppointmentId = model.AppointmentId,
                PatientComplaints = model.PatientComplaints,
                Diagnosis = model.Diagnosis,
                Treatment = model.Treatment
            };

            _context.MedicalHistory.Add(medicalHistory);
            await _context.SaveChangesAsync();
            var appointment = await _context.Appointments.FirstAsync(x => x.AppointmentId == model.AppointmentId);
            appointment.AppointmentStatusId = 3;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(MedicalHistoryViewModel model)
        {
            var currentMedicalHistory = await _context.MedicalHistory.FirstOrDefaultAsync(x => x.MedicalHistoryId == model.MedicalHistoryId);
            if (currentMedicalHistory == null)
            {
                return;
            }

            currentMedicalHistory.PatientComplaints = model.PatientComplaints;
            currentMedicalHistory.Diagnosis = model.Diagnosis;
            currentMedicalHistory.Treatment = model.Treatment;
            _context.MedicalHistory.Update(currentMedicalHistory);
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
