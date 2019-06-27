using WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void AddNew(Employee employee);
        void Delete(int id);
        void Save();

    }
}
