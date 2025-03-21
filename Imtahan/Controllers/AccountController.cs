﻿using Core.Models;
using Imtahan.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Imtahan.Controllers
{
	public class AccountController : Controller
	{
		UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async  Task<IActionResult> Register(RegisterDto registerDto)
		{
			if(!ModelState.IsValid)
			{
				return View();
			}
			User user= new User()
			{
				Name = registerDto.Name,
				Surname = registerDto.Surname,
				Email = registerDto.Email,
				UserName = registerDto.UserName,
			};
			var result= await _userManager.CreateAsync(user,registerDto.Password);
			if(!result.Succeeded)
			{
				foreach(var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
					return View();
				}
			}
			await _signInManager.SignInAsync(user, false);
			await _userManager.AddToRoleAsync(user, "Admin");
			return RedirectToAction("Index","Home");
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
				if (user == null)
				{
					ModelState.AddModelError("", "Username ve ya password yanlisdir!");
					return View();
				}
				
			}
			var result= await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,true);
			if(result.IsLockedOut)
			{
				ModelState.AddModelError("", "Birazdan yeniden cehd edin!!!!");
				return View();
			}
			if(!result.Succeeded)
			{
                ModelState.AddModelError("", "Username ve ya password yanlisdir!");
				return View();
            }
			await _signInManager.SignInAsync(user, loginDto.IsRemember);
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> CreateRole()
		{
			IdentityRole role=new IdentityRole("Admin");
			IdentityRole role1 = new IdentityRole("Member");
			await _roleManager.CreateAsync(role);
			await _roleManager.CreateAsync(role1);

			return Ok();
		}
	}
}
