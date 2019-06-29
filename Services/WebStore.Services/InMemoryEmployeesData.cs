using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        List<Employee> Employees = new List<Employee>
        {
            new Employee { Id=1, FirstName="Имя1", SurName="Фамилия1", Patronymic="Отчество1", Age=1 },
            new Employee { Id=2, FirstName="Имя2", SurName="Фамилия2", Patronymic="Отчество2", Age=2 },
            new Employee { Id=3, FirstName="Имя3", SurName="Фамилия3", Patronymic="Отчество3", Age=3 }
        };
        public void AddNew(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            if (Employees.Contains(employee)) return;
            employee.Id = Employees.Count;
            Employees.Add(employee);
        }

        public void Delete(int id) => Employees.Remove(GetById(id));
        public IEnumerable<Employee> GetAll() => Employees;
        public Employee GetById(int id) => Employees.FirstOrDefault(e => e.Id == id);
        public void Save() { }
    }
}
