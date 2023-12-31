﻿using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UsersController(UserManager<ApplicationUser> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string SearchValue= "")
        {
            List<ApplicationUser> users;
            if (string.IsNullOrEmpty(SearchValue))
                users = await _userManager.Users.ToListAsync();
            else
                users = await _userManager.Users.Where(user => user.UserName.Trim().ToLower().Contains(SearchValue.Trim().ToLower())).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            return View(viewName, user);
        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationUser applicationUser)
        {
            if(id != applicationUser.Id)
                return NotFound();
            if(ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    user.UserName = applicationUser.UserName;
                    user.NormalizedUserName = applicationUser.UserName.ToUpper();

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));

                    foreach(var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return View(applicationUser);
        }

        public async Task<IActionResult> Delete(string id, ApplicationUser applicationUser)
        {
            if(id != applicationUser.Id)
                return NotFound();

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
