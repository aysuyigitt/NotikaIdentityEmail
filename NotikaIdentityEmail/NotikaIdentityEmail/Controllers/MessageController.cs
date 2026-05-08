using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models;
using NotikaIdentityEmail.Models.MessageModels;

namespace NotikaIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public MessageController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null) return RedirectToAction("Login", "Account");

            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.SenderEmail equals u.Email.ToLower() into userGroup
                          from sender in userGroup.DefaultIfEmpty()
                          where m.ReceiverEmail == user.Email
                          join c in _context.Categories
                          on m.CategoryID equals c.CategoryID into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()
                          select new MessageWithSenderInfoViewModel
                          {
                              MessageID = m.MessageID,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Bilinmeyen",
                              SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                              CategoryName = category != null ? category.CategoryName : "Kategori Yok",
                          }).ToList();

            return View(values);
        }


        public async Task<IActionResult> Sendbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.ReceiverEmail equals u.Email into userGroup
                          from receiver in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryID equals c.CategoryID into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.SenderEmail == user.Email
                          select new MessageWithReceiverInfoViewModel
                          {
                              MessageID = m.MessageID,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              ReceiverEmail = m.ReceiverEmail,
                              ReceiverName = receiver != null ? receiver.Name : "Bilinmeyen",
                              ReceiverSurname = receiver != null ? receiver.Surname : "Kullanıcı",
                              CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                          }).ToList();

            return View(values);
        }

        public IActionResult MessageDetail(int id)
        {
            var value = _context.Messages.Where(x => x.MessageID == id).FirstOrDefault();
            return View(value);
        }

        [HttpGet]
        public IActionResult ComposeMessage()
        {
            var categoires = _context.Categories.ToList();
            ViewBag.v = categoires.Select(c=> new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            }).ToList();    
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ComposeMessage(Message message)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            message.SenderEmail = user.Email;
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("SendBox");
        }

        public async Task<IActionResult> GetMessageListByCategory(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.SenderEmail equals u.Email into userGroup
                          from sender in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryID equals c.CategoryID into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.ReceiverEmail == user.Email && m.CategoryID == id
                          select new MessageWithSenderInfoViewModel
                          {
                              MessageID = m.MessageID,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Bilinmeyen",
                              SenderSurname = sender != null ? sender.Surname : "Kullanıcı",
                              CategoryName = category != null ? category.CategoryName : "Kategori Yok"
                          }).ToList();

            return View(values);
        }
    }
}

