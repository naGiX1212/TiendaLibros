using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using TiendaLibros.Models.ViewModels;

namespace TiendaLibros.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        public AccountController(SignInManager<IdentityUser> sigInManager , UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            this.signInManager = sigInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure : false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);

                }
                ViewBag.Error = "Login no valido";
                ModelState.AddModelError(string.Empty, "Login no valido");

            }
            return View(model);
        }
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model,string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email , Email = model.Email };

                var result = await userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent : false);
                    return RedirectToLocal(returnUrl);
                }
                var errorMsg = "No se pudo crear cuenta";
                foreach (var error in result.Errors)
                {
                    errorMsg += error.Description + " ";
                }

                ViewBag.Error = errorMsg;
            }


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Book");
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(BookController.Index), "Book");
            }
        }
    }

}
