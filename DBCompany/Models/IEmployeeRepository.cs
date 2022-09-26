using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DBCompany.Models
{
    public interface IEmployeeRepository
    {
        DataTable Table { get; set; }
        void Add(Employee employee);
        void Edit(Employee employee);
        void Delete(int id);
        void UpdateRepository();

        bool CheckUniqueFullNameByOtherId(Employee employee);
        bool CheckUniqueLoginByOtherId(Employee employee);
        bool ExistId(int id);

    }
}
