using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebStore.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        IEmployeesData Employees;
        public EmployeesController(IEmployeesData employees)
        {
            this.Employees = employees;
        }

        public IActionResult Index()
        {
            return View(Employees.GetAll());
        }
        public IActionResult Details(int id)
        {
            var employee = Employees.GetById(id);
            if (employee is null) return NotFound();
            return View(employee);
        }
        [Authorize(Roles = Domain.Entities.User.RoleAdmin)]
        public IActionResult Edit(int? id)
        {
            if (id is null) return View();
            return View(Employees.GetById((int)id));
        }
        [HttpPost]
        [Authorize(Roles = Domain.Entities.User.RoleAdmin)]
        public IActionResult Edit(Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);
            if (employee.Id > 0)
            {
                var db_employee = Employees.GetById(employee.Id);
                if (db_employee is null) return NotFound();
                db_employee.FirstName = employee.FirstName;
                db_employee.SurName = employee.SurName;
                db_employee.Patronymic = employee.Patronymic;
                db_employee.Age = employee.Age;
            }
            else Employees.AddNew(employee);
            Employees.Save();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Domain.Entities.User.RoleAdmin)]
        public IActionResult Delete(int id)
        {
            var emp = Employees.GetById(id);
            if (emp is null) return NotFound();
            Employees.Delete(id);

            return RedirectToAction("Index");
        }
    }
}