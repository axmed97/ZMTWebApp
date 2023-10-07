using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.DTO;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(loginDTO.Email);
                if(checkEmail == null)
                {
                    ModelState.AddModelError("Error", "Email or Password is not valid!");
                    return View();
                }

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, loginDTO.RememberMe, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Email or Password is not valid!");
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var checkEmail = await _userManager.FindByEmailAsync(registerDTO.Email);

                if (checkEmail != null)
                {
                    ModelState.AddModelError("Error", "Email is already exists!");
                    return View();
                }

                User newUser = new()
                {
                    Email = registerDTO.Email,
                    UserName = registerDTO.Email,
                    Firstname = registerDTO.Firstname,
                    Lastname = registerDTO.Lastname,
                    PhotoUrl = "/"
                };

                var result = await _userManager.CreateAsync(newUser, registerDTO.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Error", error.Description);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
