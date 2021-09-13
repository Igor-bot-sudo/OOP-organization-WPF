namespace OrganizationStructure
{
    public class Manager : Worker
    {
        public Manager() {}

        // Конструктор с параметрами
        public Manager(string FirstName, string LastName, int Age, string Category)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;
            this.Category = Category;
            this.DepartmentID = DepartmentID;
        }

        public override int SetSalary(Department dept)
        {
            int result = 0;
            int sum = 0;

            // Суммируем зарплату всех сотрудников кроме руководителя
            if (dept.Workers != null)
            {
                foreach (var w in dept.Workers)
                {
                    if (w.Category == "Начальник отдела") continue;
                    sum += w.Salary;
                }      
            }

            // Если есть подчиненные департаменты, добавляем суммы зарплат их сотрудников
            if (dept.Departments != null)
            {
                foreach (var d in dept.Departments)
                {
                    foreach (var w in d.Workers)
                    {
                        if (w.Category == "Начальник отдела")
                        {
                            Manager m = new Manager();
                            result = m.SetSalary(d);
                            w.Salary = m.Salary;
                            sum += result;
                            break;
                        }
                    }
                }
            }

            result = sum;
            sum = (int)(sum * 0.15f);
            if (sum < 1300) sum = 1300;
            this.Salary = sum;

            // Добавляем к результату зарплату руководителя
            result += this.Salary;

            return result;
        }
    }
}
