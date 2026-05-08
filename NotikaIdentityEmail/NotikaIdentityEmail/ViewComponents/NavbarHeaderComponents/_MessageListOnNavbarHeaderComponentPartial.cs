using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.MessageModels;

namespace NotikaIdentityEmail.ViewComponents.NavbarHeaderComponents
{
    public class _MessageListOnNavbarHeaderComponentPartial : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _emailContext;

        public _MessageListOnNavbarHeaderComponentPartial(UserManager<AppUser> userManager, EmailContext emailContext)
        {
            _userManager = userManager;
            _emailContext = emailContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userValue = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEmail = userValue.Email;
            var values = from message in _emailContext.Messages
                         join user in _emailContext.Users
                         on message.SenderEmail equals user.Email
                         where message.ReceiverEmail == userEmail
                         select new MesssageListWithUserInfoViewModel
                         {
                             FullName = user.Name + " " + user.Surname,
                             ProfileImageUrl = user.ImageUrl,
                             SendDate = message.SendDate,
                             MessageDetail = message.MessageDetail,
                         };

            return View(values.ToList());
        }
    }   
}
