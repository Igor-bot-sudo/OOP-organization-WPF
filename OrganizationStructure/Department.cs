using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OrganizationStructure
{
    public class Department : IEquatable<Department>
    {
        // Название департамента
        public string Name { get; set; }

        // Идентификатор департамента
        public int DepartmentID { get; set; }

        // Список вложенных департаментов
        public List<Department> Departments { get; set; }

        // Список сотрудников департамента
        public List<Worker> Workers { get; set; }

        // Список свободных идентификаторов департаментов
        public List<int> FreeIDNumbers { get; set; }

        // Максимальный текущий идентификатор
        public int MaxID { get; set; }

        Random rand = new Random();

        public static void CreateOrganization()
        {
            string json;
            Department d, dept = new Department();
            int numID = 0;
            dept.Name = "Компания Лучшие кодеры";

            // Поток записи данных в JSON-файл
            using (StreamWriter sw = new StreamWriter("Organization.json", false, Encoding.UTF8))
            {
                d = dept.Append(dept.Name, 1, true);
                d.FreeIDNumbers = new List<int>();
                d.SetID(d, ref numID);
                d.MaxID = numID;
                json = JsonConvert.SerializeObject(d);
                sw.WriteLine(json);
            }
        }


        /// <summary>
        /// Метод присвоения департаменту идентификатора
        /// </summary>
        /// <param name="d">Департамент</param>
        /// <param name="deptID">Идентификатор</param>
        public void SetID(Department d,  ref int deptID)
        {
            d.DepartmentID = ++deptID;
            foreach (var w in d.Workers)
            {
                w.DepartmentID = d.DepartmentID;
            }

            if (d.Departments != null)
            {
                foreach (var _d in d.Departments)
                {
                    SetID(_d, ref deptID);
                }
            }
        }


        /// <summary>
        /// Метод создания департамента
        /// </summary>
        /// <param name="_d">Название департамента</param>
        /// <param name="current_depth">Текущий уровень вложенности</param>
        /// <param name="director">Логический параметр, определяющий создается отдельный департамент или вся организация</param>
        /// <returns>Созданный департамент</returns>
        public Department Append(string deptName, int current_depth, bool director)
        {
            Department d = new Department();
            d.Name = deptName;
            d.Workers = new List<Worker>();            

            if (!director)
            {
                // Рекурсивно создаем департаменты нижнего уровня
                if (current_depth > 0)
                {
                    current_depth--;
                    d.Departments = new List<Department>();
                    for (int i = 1; i < rand.Next(2, 5); i++)
                    {
                        Department vd = Append(d.Name + i.ToString(), current_depth, false);
                        d.Departments.Add(vd);
                    }
                }
                // Добавляем сотрудников
                CreateWorkers(d);
            }
            else
            {
                d.Departments = new List<Department>();
                // Создаем подразделения
                for (int i = 1; i < 21; i++)
                {
                    Department vd = Append("Отдел_" + i.ToString(), rand.Next(1, 4), false);
                    d.Departments.Add(vd);
                }

                // Добавляем директора
                Chief chief = new Chief();
                chief.FirstName = "Имя_директора";
                chief.LastName = "Фамилия_директора";
                chief.Age = rand.Next(40, 65);
                chief.Category = "Директор организации";
                chief.SetSalary(d);
                d.Workers.Add(chief);
            }
            return d;
        }


        /// <summary>
        /// Метод добавления сотрудников в департамент
        /// </summary>
        /// <param name="dept">Департамент</param>
        private void CreateWorkers(Department dept)
        {
            // Добавляем сотрудников
            for (int i = 1; i < rand.Next(5, 21); i++)
            {
                if (rand.Next(2) == 0)
                {
                    Worker coder = new Coder();
                    coder.FirstName = "Имя_" + i.ToString();
                    coder.LastName = "Фамилия_" + i.ToString();
                    coder.Age = rand.Next(23, 45);
                    coder.Category = "Кодер";
                    coder.SetSalary(dept);
                    dept.Workers.Add(coder);
                }
                else
                {
                    Worker intern = new Intern();
                    intern.FirstName = "Имя_" + i.ToString();
                    intern.LastName = "Фамилия_" + i.ToString();
                    intern.Age = rand.Next(20, 35);
                    intern.Category = "Интерн";
                    intern.SetSalary(dept);
                    dept.Workers.Add(intern);
                }
            }

            Manager manager = new Manager("Имя_руководителя", "Фамилия_руководителя",
                rand.Next(30, 45), "Начальник отдела");
            // Для исключения повторных действий зарплата руководителей департаментов
            // начисляется при рекурсивном вычислении зарплаты директора организации
            manager.Salary = 0;
            dept.Workers.Add(manager);
        }

        /// <summary>
        /// Реализация метода сравнения двух департаментов
        /// </summary>
        /// <param name="dept">Сравниваемый департамент</param>
        /// <returns>Очевидно</returns>
        public bool Equals(Department dept)
        {
            return dept.Name == Name && dept.DepartmentID == DepartmentID;
        }


        /// <summary>
        /// Метод удаления департамента из структуры
        /// </summary>
        /// <param name="_organization">Организация</param>
        /// <param name="_dept">Удаляемый департамент</param>
        public static void DeleteDepartment(Department _organization, Department _dept)
        {
            Department _vd = null;

            if (_organization.Departments != null)
            {
                foreach (var item in _organization.Departments)
                {
                    if (item.Equals(_dept))
                    {
                        _vd = item;
                        break;
                    }
                    else
                    {
                        if (item.Departments != null)
                        {
                            DeleteDepartment(item, _dept);
                        }
                    }
                }
            }

            if (_vd != null) _organization.Departments.Remove(_vd);
        }

        /// <summary>
        /// Метод удаления данных сотрудника из структуры
        /// </summary>
        /// <param name="dept">Департамент, в котором работает сотрудник</param>
        /// <param name="w">Сотрудник</param>
        public static void DeleteWorker(Department dept, Worker w)
        {
            Worker _vw = null;

            foreach (var worker in dept.Workers)
            {
                if (w.Equals(worker))
                {
                    _vw = worker;
                    break;
                }
            }

            if (_vw != null)
            {
                dept.Workers.Remove(_vw);
            }
        }


        /// <summary>
        /// Метод перерасчета зарплаты сотрудников после внесения изменений в структуру
        /// </summary>
        /// <param name="organization">Организация</param>
        public static void Recount(Department organization)
        {
            if (organization.Workers != null && organization.Workers[0].Category == "Директор организации")
            {
                Chief _c = new Chief();
                _c.SetSalary(organization);
                organization.Workers[0].Salary = _c.Salary;
            }
        }
    }
}
