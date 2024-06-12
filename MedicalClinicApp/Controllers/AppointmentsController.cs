using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentsController(AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AppointmentViewModel> appointments = new List<AppointmentViewModel>();
            var currentUserEmail = User.Claims.First(x => x.Type == "Email").Value;
            switch(User.Claims.First(x => x.Type == "Role").Value)
            {
                case "1":
                    appointments = await _appointmentRepository.GetAll();
                    break;
                case "2":
                    appointments = await _appointmentRepository.GetByEmployee(currentUserEmail);
                    break;
                case "3":
                    appointments = await _appointmentRepository.GetByPatient(currentUserEmail);
                    break;
            }

            return View(appointments);
        }

        [Authorize(Policy = "Patient")]
        public async Task<IActionResult> SelectADoctor()
        {
            ViewBag.Employees = await _appointmentRepository.GetEmployeesList();
            return View();
        }

        [Authorize(Policy = "Patient")]
        [HttpPost]
        public async Task<IActionResult> SelectADoctor(EmployeeSelectionViewModel model)
        {
            if (model.Date.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("Date", "Нельзя записаться в прошлое...");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(SelectSlot), new { model.EmployeeId, model.Date });
            }

            ViewBag.Employees = await _appointmentRepository.GetEmployeesList();
            return View(model);
        }

        [Authorize(Policy = "Patient")]
        [HttpGet]
        public async Task<IActionResult> SelectSlot(int EmployeeId, DateTime Date)
        {
            ViewBag.TimeSlots = await _appointmentRepository.GetTimeSlots(EmployeeId, Date);
            var appointment = new TimeSlotSelectionViewModel
            {
                EmployeeId = EmployeeId,
                PatientId = await _appointmentRepository.GetPatientIdByEmail(User.Claims.First(x => x.Type == "Email").Value),
            };

            return View(appointment);
        }

        [Authorize(Policy = "Patient")]
        [HttpPost]
        public async Task<IActionResult> SelectSlot(TimeSlotSelectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _appointmentRepository.Create(model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TimeSlots = _appointmentRepository.GetTimeSlots(model.EmployeeId, model.SelectedSlot);
            return View(model);
        }

        [Authorize(Policy = "Patient")]
        [HttpGet]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            await _appointmentRepository.CancelAppointment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
