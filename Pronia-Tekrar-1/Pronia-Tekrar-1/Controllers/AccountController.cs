using Microsoft.AspNetCore.Mvc;
using Pronia_Tekrar_1.Helpers.Enums;
using Pronia_Tekrar_1.ViewModels;
using Pronia_Tekrar_1.ViewModels.Account;

namespace Pronia_Tekrar_1.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm Vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser()
            {
                Name = Vm.Name,
                Surname = Vm.Surname,
                Email = Vm.Email,
                UserName = Vm.UserName,
            };

            var result = await _userManager.CreateAsync(appUser, Vm.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            
            await _userManager.AddToRoleAsync(user,UserRole.Admin.ToString());

            return RedirectToAction("LogOut");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByEmailAsync(loginVm.EmailOrUsername) ?? await _userManager.FindByNameAsync(loginVm.EmailOrUsername);

            if (user == null)
            {
                ModelState.AddModelError("", "UserName Or Email ");
                return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVm.Password, true);
            if (!result.IsLockedOut)
            {
                ModelState.AddModelError("", "az sonra tekrardan sinayin");
                return View();
            }

            if (result.Succeeded)
            {
                ModelState.AddModelError("", "az sonra tekrardan sinayin");
                return View();
            }



            await _signInManager.SignInAsync(user, loginVm.Remember);

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");


        }
        public async IActionResult CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = item.ToString(),
                });


                return RedirectToAction("Index", "Home");
            }
        }
    }
}
