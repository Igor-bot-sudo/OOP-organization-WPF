using System;

namespace OrganizationStructure
{
    public class Worker : AbstractWorker,  IEquatable<Worker>
    {
        public override int SetSalary(Department d)
        {
            return 0;
        }

        /// <summary>
        /// Реализация метода сравнения двух работников
        /// </summary>
        /// <param name="worker">Сравниваемый работник</param>
        /// <returns>Результат понятен</returns>
        public bool Equals(Worker worker)
        {
            return worker.FirstName == this.FirstName && worker.LastName == this.LastName
                    && worker.DepartmentID == this.DepartmentID;
        }
    }
}
