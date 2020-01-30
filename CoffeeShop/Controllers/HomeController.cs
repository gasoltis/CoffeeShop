using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Shop()
        {
            var model = new List<Items>();
            using (var db = new ShopDBContext())
            {
                model = db.Items.ToList();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Shop(int itemId)
        {
            using (var db = new ShopDBContext())
            {
                var item = db.Items.FirstOrDefault(i => i.Id == itemId);
                var username = HttpContext.Session.GetString("session_username");
                if (!string.IsNullOrEmpty(username))  
                {
                    var user = db.Users.FirstOrDefault(u => u.Username == username);
                    {
                        if (user != null)
                        {
                            if (item != null)
                            {
                                var itemPrice = item.Price;
                                var userMoney = user.UserFunds;

                                if (userMoney > item.Price)
                                {
                                    user.UserFunds = user.UserFunds - itemPrice;
                                    item.Quantity = item.Quantity - 1;
                                    db.SaveChanges();
                                    return RedirectToAction("Shop", "Home");
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "You don't have enough money.";
                                    return View("Error");
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "That is not a valid item.";
                                return View("Error");
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Please log in again.";
                            return View("Error");
                        }
                    }
                }
            }
            return View("Error");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            using (var db = new ShopDBContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    //check password if user is not null
                    if (password == user.Password)
                    {
                        HttpContext.Session.SetString("session_username", user.Username);

                        //TODO: CHANGE TO APPROPRIATE VIEW TO GO TO 
                        return View("RegisterSuccess");  //RegisterSuccess returned here originally
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Password is incorrect";
                        return View();
                    }
                }
                else // user was Null, not found in database
                {
                    ViewBag.ErrorMessage = "User not Found";
                    return View();
                }
            }
        }

        [HttpPost]
        public IActionResult MakeNewUser(Users u)
        {
            using (var db = new ShopDBContext())
            {
                var newUser = new Users
                {
                    Email = u.Email,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Username = u.Username,
                    Password = u.Password,
                    UserFunds = 100
                };

                db.Users.Add(newUser);
                if (db.SaveChanges() > 0)
                {
                    //Do the work to create new user; 
                    //if successful return view Register Success; if not (go back to register page or another view)
                    return View("RegisterSuccess");
                }
                else
                {
                    // Was not able to save user to database for whatever reason.
                    ViewBag.ErrorMessage("Was not successful.");
                    return View("Register");
                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
            string firstname,
            string lastname,
            string username,
            string email,
            string password,
            string coffee,
            string passwordtwo
            )
        {
            ViewBag.Firstname = firstname;
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
