using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationStructure
{
    public abstract class AbstractWorker
    {
        // Имя сотрудника
        public string FirstName { get; set; }

        // Фамилия сотрудника
        public string LastName { get; set; }

        // Возраст
        public int Age { get; set; }

        // Должность сотрудника
        public string Category { get; set; }

        // Размер оплаты труда сотрудника
        public int Salary { get; set; }

        // Идентификатор департамента
        public int DepartmentID { get; set; }

        /// <summary>
        /// Метод начисления зарплаты сотрудника
        /// </summary>
        /// <param name="d">Департамент</param>
        /// <returns>Сумма зарплат всех сотрудников департамента</returns>
        public abstract int SetSalary(Department d);
    }
}
