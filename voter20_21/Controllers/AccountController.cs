using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using voter20_21.Services;
using voter20_21.Models;
namespace voter20_21.Controllers
{
    public class AccountController : Controller
    {
        AccountService accService;
        VoterService voterService;

        public AccountController(AccountService _accountService, VoterService _voterService)
        {
            accService = _accountService;
            voterService = _voterService;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            //lehet ilyesmire nekem is szükségem lesz: ViewBag.vmi = vmiService.vmi.ToArray();
        }
        // GET: AccountController
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", user);
            }
            if (!accService.Login(user))
            {
                ModelState.AddModelError("", "Hibás felhasználónév vagy jelszó");
                return View("Login", user);
            }
            //munkamenetbe felvesszük a felhasználó e-mail címét:
            //megjegyzés: a session szerveroldalon tárolódik, ezért nem kell titkosítani ezt
            HttpContext.Session.SetString("user", user.email);

            ViewBag.email = user.email;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public IActionResult Register(RegistrationViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }
            if (!accService.Register(user))
            {
                ModelState.AddModelError("email", "A megadott e-mail cím már használatban van.");
                return View("Register", user);
            }
            //ViewBag.Information = "A regisztráció sikeres volt, mostmár be tud jelentkezni.";
            //ha már be volt jelentkezve egy felhasználó, akkor kijelentkeztetjük:
            if(HttpContext.Session.GetString("user") != null)
            {
                HttpContext.Session.Remove("user");
            }
            return RedirectToAction("Login");
        }
        public IActionResult Logout()
        {
            //ha be volt jelentkezve
            if(HttpContext.Session.GetString("user") != null)
            {
                HttpContext.Session.Remove("user");
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
