using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DotnetProjet5.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Cet email est déjà utilisé. Veuillez en choisir un autre.");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        //        var result = await _userManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            // Assign the "user" role to the newly created account
        //            await _userManager.AddToRoleAsync(user, "user");

        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}
    }
  
}
