using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Monahov.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Monahov.Controllers

{
    
    public class EmployeeController:Controller
    {
        public ApplicationContext db;
        private readonly UserManager<User> _userManager;

        public EmployeeController(ApplicationContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager= userManager;

            if(db.Employees.Count() == 0)
            {
                Employee employee1 = new Employee 
                { 
                    FirstName = "Abragim",
                     LastName = "Voinu",
                     Position = "Dvornik", 
                     Salary= 10000,
                     Premium = 0, 
                     IsDismissed = false};
                Employee employee2 = new Employee
                {
                    FirstName = "Wragim",
                    LastName = "Aoinu",
                    Position = "Dvornik",
                    Salary = 10000,
                    Premium = 0,
                    IsDismissed = true
                };
                db.Employees.AddRange(employee1, employee2);
                db.SaveChanges();
            }
            
        }
        public async Task<IActionResult> Index(SortEmployee sortEmployee = SortEmployee.NameAsc)
        {
            if (User.IsInRole("admin") || User.IsInRole("manager"))
            {
                IQueryable<Employee> employees = db.Employees;
                
                ViewData["NameSort"] = sortEmployee == SortEmployee.NameAsc ? SortEmployee.NameDesc : SortEmployee.NameAsc;
                ViewData["PositionSort"] = sortEmployee == SortEmployee.PositionAsc ? SortEmployee.PositionDesc : SortEmployee.PositionAsc;
                ViewData["IsDismissed"] = sortEmployee == SortEmployee.IsDismissedAsc ? SortEmployee.IsDismissedDesc : SortEmployee.IsDismissedAsc;

                employees = sortEmployee switch
                {
                    SortEmployee.NameDesc => employees.OrderByDescending(s=>s.FirstName),
                    SortEmployee.PositionAsc => employees.OrderBy(s=>s.Position),
                    SortEmployee.PositionDesc => employees.OrderByDescending(s=>s.Position),
                    SortEmployee.IsDismissedDesc => employees.OrderByDescending(s=>s.IsDismissed),
                    SortEmployee.IsDismissedAsc => employees.OrderBy(s=>s.IsDismissed),
                    _ => employees.OrderBy(s => s.FirstName),
                };
                IndexViewModel viewModel = new IndexViewModel
                {
                    Employees = await employees.AsNoTracking().ToListAsync(),
                    SortViewModel = new SortViewModel(sortEmployee)

                };
                
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Stop", "Home");
            }
        }
        [Authorize(Roles = "admin,manager")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(Employee user)
        {
            db.Employees.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Employee user = await db.Employees.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(Employee user)
        {
            db.Employees.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Employee user = await db.Employees.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Employee user = await db.Employees.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Employees.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
