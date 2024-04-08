using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyNotes.Models;
using MyNotes.ViewModel;
using System.Drawing.Printing;

namespace MyNotes.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
    [HttpGet]
    public IActionResult Register() 
        {
            return View(new RegisterViewModel());        
        }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            // this statement is not executing so its not registering user
            if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    return RedirectToAction(nameof(Index), "Note");
                }
        }
        return View(model);
    }

        public IActionResult Changepass()
        {
            return View(new ChangepassViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Changepass(ChangepassViewModel model)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    //return View();

                    return RedirectToAction(nameof(Index), "Note");
                }
                else
                {
                    // Password change failed, handle the error
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                // User not found, handle the error
                ModelState.AddModelError(string.Empty, "User not found.");
            }

            // If we reach this point, there was an error, return the view with the model
            return View(model);
        }




        [HttpGet("Login")]
        public IActionResult Login(string? returnUrl) 
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                    model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (returnUrl == null)
                    {
                    return RedirectToAction(nameof(Index), "Note");
                    }
                    else
                    {
                        return LocalRedirect(returnUrl);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }




    }


}
