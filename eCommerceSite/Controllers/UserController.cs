﻿using eCommerceSite.Data;
using eCommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // Check if username/email is unique
                bool isEmailTaken = await (from account in _context.UserAccounts
                                     where account.Email == reg.Email
                                     select account).AnyAsync();

                // if so, Add custom error message and send back to view.
                if (isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That email is taken!");
                }

                bool isUserNameTaken = await (from account in _context.UserAccounts
                                              where account.Username == reg.Username
                                              select account).AnyAsync();

                if (isUserNameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Username), "That Username is taken!");
                }
                
                if (isEmailTaken || isUserNameTaken)
                {
                    return View(reg);
                }

                // Map data to user account object
                UserAccount acc = new UserAccount()
                {
                    DateOfBirth = reg.DateOfBirth,
                    Email = reg.Email,
                    Password = reg.Password,
                    Username = reg.Username
                };

                // Add to database
                _context.UserAccounts.Add(acc);
                await _context.SaveChangesAsync();

                LogUserIn(acc.UserId);

                // Redirect to Homepage
                return RedirectToAction("Index", "Home");
            }
            return View(reg);
        }

        public IActionResult Login()
        {
            // Check if user is logged in
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //UserAccount account = await (from u in _context.UserAccounts
            //                       where (u.Username == model.UsernameOrEmail
            //                       || u.Email == model.UsernameOrEmail)
            //                       && u.Password == model.Password
            //                       select u).SingleOrDefaultAsync();
            UserAccount account = await _context.UserAccounts
                .Where(userAcc => (userAcc.Username == model.UsernameOrEmail ||
                       userAcc.Email == model.UsernameOrEmail) &&
                       userAcc.Password == model.Password)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                // Custom Error message
                ModelState.AddModelError(string.Empty, "Credentials were not found");

                return View(model);
            }

            // Log user into website
            LogUserIn(account.UserId);

            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            // Removes all the current session data
            HttpContext.Session.Clear();

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }
    }
}
