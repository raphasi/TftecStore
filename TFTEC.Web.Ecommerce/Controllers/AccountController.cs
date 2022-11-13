using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TFTEC.Web.Ecommerce.ViewModel;

namespace TFTEC.Web.Ecommerce.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user,
                    loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }
            ModelState.AddModelError("", "Falha ao realizar o login!!");
            return View(loginVM);
        }//

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel registroVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registroVM.UserName };
                var result = await _userManager.CreateAsync(user, registroVM.Password);

                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    if (result.Errors.Count() == 1)
                        this.ModelState.AddModelError("Registro", "Falha ao cadastrar usuário");
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            var mensagem = string.Empty;

                            if (item.Description.Contains("Passwords must be at least 6 characters."))
                                mensagem = "As senhas devem ter pelo menos 6 caracteres.";
                            if (item.Description.Contains("Passwords must have at least one non alphanumeric character."))
                                mensagem = "As senhas devem ter pelo menos um caractere não alfanumérico.";
                            if (item.Description.Contains("Passwords must have at least one digit ('0'-'9')."))
                                mensagem = "As senhas devem ter pelo menos um dígito ('0'-'9').";
                            if (item.Description.Contains("Passwords must have at least one uppercase ('A'-'Z')."))
                                mensagem = "As senhas devem ter pelo menos uma letra maiúscula ('A'-'Z').";

                            this.ModelState.AddModelError("Registro", mensagem);
                        }
                    }
                }
            }
            return View(registroVM);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
