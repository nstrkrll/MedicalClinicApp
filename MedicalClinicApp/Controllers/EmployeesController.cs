using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAll();
            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Posts = await _employeeRepository.GetPostList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (await _employeeRepository.CheckIfExists(model.Email))
            {
                ModelState.AddModelError("Email", "Такой email уже используется");
            }

            if (ModelState.IsValid)
            {
                await _employeeRepository.Create(model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Posts = await _employeeRepository.GetPostList();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.Get(id);
            ViewBag.Posts = await _employeeRepository.GetPostList();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            var currentUser = await _employeeRepository.Get((int)model.EmployeeId);
            if (await _employeeRepository.CheckIfExists(model.Email) && currentUser.Email != model.Email)
            {
                ModelState.AddModelError("Email", "Такой email уже используется");
            }

            if (ModelState.IsValid)
            {
                await _employeeRepository.Edit(model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Posts = await _employeeRepository.GetPostList();
            return View(model);
        }
    }
}
