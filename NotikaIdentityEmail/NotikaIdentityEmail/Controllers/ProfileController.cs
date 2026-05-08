using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.IdentityModels;

namespace NotikaIdentityEmail.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var values  = await _userManager.FindByNameAsync(User.Identity.Name); //User.Identity üzerinden giriş yapan kullanıcının username bilgisi alınır.
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            userEditViewModel.Surname = values.Surname;
            userEditViewModel.PhoneNumber = values.PhoneNumber;
            userEditViewModel.UserName = values.UserName;
            userEditViewModel.Name = values.Name;
            userEditViewModel.ImageUrl = values.ImageUrl;
            userEditViewModel.City = values.City;
            userEditViewModel.Email = values.Email;
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditViewModel model)
        {
            if(model.Password == model.PasswordConfirm)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName; 
                user.ImageUrl = model.ImageUrl;
                user.Email = model.Email;
                user.City = model.City;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                await _userManager.UpdateAsync(user);
            }
            return View();
        }
    }
}
