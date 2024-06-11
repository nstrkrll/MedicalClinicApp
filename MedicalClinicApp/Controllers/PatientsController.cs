using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PatientsController : Controller
    {
        private readonly PatientRepository _patientRepository;

        public PatientsController(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await _patientRepository.GetAll();
            return View(patients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (await _patientRepository.CheckIfExists(model.Email))
            {
                ModelState.AddModelError("Email", "Такой email уже используется");
            }

            if (ModelState.IsValid)
            {
                await _patientRepository.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var patients = await _patientRepository.Get(id);
            return View(patients);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PatientViewModel model)
        {
            var currentUser = await _patientRepository.Get((int)model.PatientId);
            if (await _patientRepository.CheckIfExists(model.Email) && currentUser.Email != model.Email)
            {
                ModelState.AddModelError("Email", "Такой email уже используется");
            }

            if (ModelState.IsValid)
            {
                await _patientRepository.Edit(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}