using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Authorize]
    public class MedicalHistoryController : Controller
    {
        private readonly MedicalHistoryRepository _medicalHistoryRepository;

        public MedicalHistoryController(MedicalHistoryRepository medicalHistoryRepository)
        {
            _medicalHistoryRepository = medicalHistoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<MedicalHistoryPreviewViewModel> history = new List<MedicalHistoryPreviewViewModel>();
            var currentUserEmail = User.Claims.First(x => x.Type == "Email").Value;
            switch (User.Claims.First(x => x.Type == "Role").Value)
            {
                case "1":
                    history = await _medicalHistoryRepository.GetAll();
                    break;
                case "2":
                    history = await _medicalHistoryRepository.GetByEmployee(currentUserEmail);
                    break;
                case "3":
                    history = await _medicalHistoryRepository.GetByPatient(currentUserEmail);
                    break;
            }

            return View(history);
        }

        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Appointments = await _medicalHistoryRepository.GetAppointmentsByEmployee(User.Claims.First(x => x.Type == "Email").Value);
            return View();
        }

        [Authorize(Policy = "Employee")]
        [HttpPost]
        public async Task<IActionResult> Create(MedicalHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _medicalHistoryRepository.Create(model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Appointments = await _medicalHistoryRepository.GetAppointmentsByEmployee(User.Claims.First(x => x.Type == "Email").Value);
            return View(model);
        }

        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var medicalhistory = await _medicalHistoryRepository.Get(id);
            return View(medicalhistory);
        }

        [Authorize(Policy = "Employee")]
        [HttpPost]
        public async Task<IActionResult> Edit(MedicalHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _medicalHistoryRepository.Edit(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}