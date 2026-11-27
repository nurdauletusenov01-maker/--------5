using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Students") : View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(model.Email), "Бұл Email тіркелген.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName.Trim(),
            LastName = model.LastName.Trim()
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            IdentityResult roleResult;
            try
            {
                roleResult = await _userManager.AddToRoleAsync(user, "Student");
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(
                    exception,
                    "Student рөлін тағайындау кезінде қате шықты: {Email}",
                    model.Email);
                roleResult = IdentityResult.Failed(
                    new IdentityError { Description = "Student рөлін тағайындау сәтсіз аяқталды." });
            }

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    foreach (var error in deleteResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            $"Пайдаланушыны кері қайтару қатесі: {error.Description}");
                    }
                    _logger.LogError(
                        "Student рөлін тағайындау сәтсіз болып, пайдаланушыны жою мүмкін болмады: {Email}. Қателер: {Errors}",
                        model.Email,
                        string.Join("; ", deleteResult.Errors.Select(error => error.Description)));
                }
                else
                {
                    _logger.LogInformation(
                        "Student рөлін тағайындау сәтсіз болғандықтан пайдаланушы кері жойылды: {Email}",
                        model.Email);
                }

                _logger.LogError("Пайдаланушыға Student рөлі тағайындалмады: {Email}", model.Email);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Students");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
            _logger.LogWarning("Тіркелу қатесі ({Email}): {Error}", model.Email, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Students");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Students");
        }

        _logger.LogWarning("Қате Email немесе құпия сөзмен кіру әрекеті: {Email}", model.Email);
        ModelState.AddModelError(string.Empty, "Email немесе құпия сөз қате.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
