using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData employeesData;

        public EmployeesController(IEmployeesData employeesData)
        {
            this.employeesData = employeesData;
        }
        [HttpPost, ActionName("Post")]
        public void AddNew([FromBody]Employee employee)
        {
            employeesData.AddNew(employee);
        }
        [HttpDelete("id")]
        public void Delete(int id)
        {
            employeesData.Delete(id);
        }
        [HttpGet, ActionName("getall")]
        public IEnumerable<Employee> GetAll()
        {
            return employeesData.GetAll();
        }
        [HttpGet("{id}"), ActionName("Get")]
        public Employee GetById(int id)
        {
            return employeesData.GetById(id);
        }
        [NonAction]
        public void Save()
        {
            employeesData.Save();
        }
        [HttpPut("{id}"), ActionName("Put")]
        public Employee Update(int id, Employee employee)
        {
            return employeesData.Update(id, employee);
        }
    }
}