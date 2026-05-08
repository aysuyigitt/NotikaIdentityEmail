using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;

namespace NotikaIdentityEmail.ViewComponents.NavbarHeaderComponents
{
    public class NotificationListOnNavbarComponentPartial : ViewComponent
    {
        private readonly EmailContext _emailContext;

        public NotificationListOnNavbarComponentPartial(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }

        public IViewComponentResult Invoke()
        {
            var values = _emailContext.Notifications.ToList();
            return View(values);
        }
    }
}
