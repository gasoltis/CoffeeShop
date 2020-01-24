using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoffeeShop.Models;

namespace CoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        //need one action to load our RegistrationPage, also need a view

        [HttpPost]
        public IActionResult Welcome(
            string firstname,
            string middleinitial,
            string lastname,
            string username,
            string email,
            string password,
            string coffee,
            string passwordtwo
            )
        {
            ViewBag.Firstname = firstname;
            ViewBag.Middleinitial = middleinitial;
            ViewBag.Lastname = lastname;
            ViewBag.Username = username;
            ViewBag.Email = email;
            ViewBag.Coffee = coffee;
            ViewBag.Password = password;
           
            if (password == passwordtwo)
            {
                return View();
            }
            else
            {
                return PasswordError();
            }

        }

        public IActionResult PasswordError()
        {
            return View("PasswordError");
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            ViewBag.Firstname = "Gina";
            ViewBag.Middleinitial = "A";
            ViewBag.Lastname = "Soltis";
            ViewBag.Username = "geener";
            ViewBag.Email = "geeneropolisatgmail";
            ViewBag.Password = "qwerty";
            ViewBag.Coffee = "beans";

            return View();
        }
        public IActionResult Privacy()
        //need one action to take those user inputs, and display the user name, in a new view

        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
