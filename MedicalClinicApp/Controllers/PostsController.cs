using MedicalClinicApp.Models.ViewModels;
using MedicalClinicApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PostsController : Controller
    {
        private readonly PostRepository _postRepository;

        public PostsController(PostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAll();
            return View(posts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _postRepository.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postRepository.Get(id);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel model)
        {
            var currentUser = await _postRepository.Get((int)model.PostId);
            if (ModelState.IsValid)
            {
                await _postRepository.Edit(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}