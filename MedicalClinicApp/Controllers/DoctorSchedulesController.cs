using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class DoctorSchedulesController : Controller
    {
        private readonly DoctorScheduleRepository _doctorScheduleRepository;

        public DoctorSchedulesController(DoctorScheduleRepository doctorScheduleRepository)
        {
            _doctorScheduleRepository = doctorScheduleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _doctorScheduleRepository.GetAll();
            return View(schedules);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var schedules = await _doctorScheduleRepository.Get(id);
            return View(schedules);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _doctorScheduleRepository.Edit(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}