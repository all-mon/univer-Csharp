using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using CP_AuthCookieApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CP_AuthCookieApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }
        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            string info;
            if (User.IsInRole("user")) info = "У вас ограниченные права доступа.";
            else if (User.IsInRole("admin")) info = "У вас есть полный доступ";
            else info = "Error";
            return Content($"Ваша роль: {role}\n{info}");
        }
        [Authorize(Roles = "admin")]
        public async Task <IActionResult> Admin()
        {
            return View(await db.Users.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            user.RoleId = 2;
            user.DateOfRegistration = DateTime.Now;
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Admin");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            user.RoleId = 2;
            user.DateOfRegistration = DateTime.Now;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Admin");
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null) 
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Admin");
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
