using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;

namespace NotikaIdentityEmail.Controllers
{
    public class CommentController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _manager;

        public CommentController(EmailContext context, UserManager<AppUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public IActionResult UserComments()
        {
            var value = _context.Comments.Include(x => x.AppUser).ToList();
            return View(value);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserCommentList()
        {
            var value = _context.Comments.Include(x => x.AppUser).ToList();
            return View(value);
        }

        [HttpGet]
        public PartialViewResult CreateComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            var user = await _manager.FindByNameAsync(User.Identity.Name);
            comment.AppUserID = user.Id;
            comment.CommentDate = DateTime.Now;
            comment.CommentStatus = "Yorum bekleniyor";
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("UserCommentList");

        }
    }
}
