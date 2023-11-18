﻿using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RolesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<ApplicationRole> roleManager,
                               ILogger<RolesController> logger,
                               UserManager<ApplicationUser> userManager) 
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(role);

                if(result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(role);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id == null)
                return NotFound();

            var roles = await _roleManager.FindByIdAsync(id);

            if (roles == null)
                return NotFound();
            
            return View(viewName, roles);
        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationRole applicationRole)
        {
            if(id != applicationRole.Id)
                return NotFound();

            if(ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    role.Name = applicationRole.Name;
                    role.NormalizedName = applicationRole.Name.ToUpper();

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return View(applicationRole);
        }
        public async Task<IActionResult> Delete(string id, ApplicationRole applicationRole)
        {
            if(id != applicationRole.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    var result = await _roleManager.DeleteAsync(role);

                    if(result.Succeeded)
                        return RedirectToAction(nameof(Index));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRole);
        }

        public async Task<IActionResult> AddOrRemoveUsers (string roleId)
        {
            var role = await _roleManager.FindByIdAsync (roleId);

            if(role == null)
                return NotFound();

            ViewBag.RoleId = roleId;

            var users = new List<UserInRoleViewModel>();

            foreach(var user in await _userManager.Users.ToListAsync())
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                users.Add(userInRole);
            }

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> users, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);

                    if (appUser != null)
                    {
                        if (user.IsSelected && !(await _userManager.IsInRoleAsync(appUser, role.Name)))
                            await _userManager.AddToRoleAsync(appUser, role.Name);

                        else if (!user.IsSelected && (await _userManager.IsInRoleAsync(appUser, role.Name)))
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                    }
                }

                return RedirectToAction(nameof(Update), new {id = roleId});
            }

            return View(users);
        }
    }
}
