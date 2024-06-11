using MedicalClinicApp.Models;
using MedicalClinicApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinicApp.Repositories
{
    public class PostRepository : IDisposable
    {
        private bool _disposed = false;
        private readonly MedicalClinicDBContext _context;

        public PostRepository(MedicalClinicDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostViewModel>> GetAll()
        {
            return await _context.Posts
                .Select(x => new PostViewModel
                {
                    PostId = x.PostId,
                    Name = x.Name,
                })
                .ToListAsync();
        }

        public async Task<PostViewModel> Get(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
            return new PostViewModel
            {
                PostId = post.PostId,
                Name = post.Name
            };
        }

        public async Task Create(PostViewModel model)
        {
            var post = new Post
            {
                PostId = model.PostId,
                Name = model.Name
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(PostViewModel model)
        {
            var currentPost = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == model.PostId);
            if (currentPost == null)
            {
                return;
            }

            currentPost.Name = model.Name;
            _context.Posts.Update(currentPost);
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
