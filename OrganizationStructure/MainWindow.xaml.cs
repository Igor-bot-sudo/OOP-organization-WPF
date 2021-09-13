using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace OrganizationStructure
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Department organization;

        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обработчик события двойного клика на элементе древовидного списка
        /// </summary>
        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Department d = null;

            if (OrganizationView.SelectedItem != null)
            {
                string str = OrganizationView.SelectedItem.ToString();

                Regex regex = new Regex(@"(.*ID департамента: )(.*)( Items.Count.*)");
                Match match = regex.Match(str);
                if (match != null) str = match.Groups[2].ToString();

                int id = Int32.Parse(str);
                d = FindDeptByID(organization, id);

                EditWindow ew = new EditWindow();

                ew.Dispatcher.Invoke(() => 
                { 
                    ew.tv = OrganizationView;
                    ew.organization = organization;
                    ew.dept = d;
                    ew.mainWindow = this;
                });

                ew.ShowDialog();
            }
        }

        /// <summary>
        /// Метод поиска департамента в структуре по его идентификатору
        /// </summary>
        /// <param name="d">Организация</param>
        /// <param name="id">Идентификатор департамента</param>
        /// <returns>Искомый департамент</returns>
        private Department FindDeptByID(Department d, int id)
        {
            Department result = null;

            if (d.DepartmentID == id)
            {
                result = d;
            }
            else
            {
                if (d.Departments != null)
                {
                    foreach (var item in d.Departments)
                    {
                        result = FindDeptByID(item, id);
                        if (result != null) break;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Метод рекурсивного вывода структуры департамента
        /// </summary>
        /// <param name="dept">Департамент</param>
        /// <param name="item">Элемент древовидного списка</param>
        private void GetSubDepartments(Department dept, TreeViewItem item)
        {
            if (dept.Departments != null)
            {
                foreach (var d in dept.Departments)
                {
                    TreeViewItem subDeptItem = new TreeViewItem();
                    subDeptItem.Header = "Название: " + d.Name + "; ID департамента: " + d.DepartmentID;
                    subDeptItem.MouseDoubleClick += TreeViewItem_Selected;
                    GetSubDepartments(d, subDeptItem);
                    item.Items.Add(subDeptItem);
                }
            }

            if (dept.Workers != null)
            {
                foreach (var w in dept.Workers)
                {
                    TreeViewItem subItem = new TreeViewItem();
                    subItem.Header = "Имя: " + w.FirstName + ";  Фамилия: " + w.LastName + ";  Возраст: " + w.Age + ";  Должность: " + w.Category + ";  Зарплата: " + w.Salary + ";  ID департамента: " + w.DepartmentID;
                    item.Items.Add(subItem);
                }
            }
        }


        /// <summary>
        /// Метод вывода структуры организации из JSON-файла
        /// </summary>
        private void ShowStructure()
        {
            using (StreamReader sr = new StreamReader("Organization.json"))
            {
                while (!sr.EndOfStream)
                {
                    string json = sr.ReadLine();
                    organization = JsonConvert.DeserializeObject<Department>(json);
                    TreeViewItem item = new TreeViewItem();
                    item.Header = organization.Name;
                    GetSubDepartments(organization, item);
                    OrganizationView.Items.Add(item);             
                }
            }

            TreeViewItem treeViewItem = null;
            foreach (var ovi in OrganizationView.Items)
            {
                treeViewItem = ovi as TreeViewItem;
                treeViewItem.IsExpanded = true;

            }
        }

        private void CreateOrganizationButton_Click(object sender, RoutedEventArgs e)
        {
            Department.CreateOrganization();
            ShowStructure();
            AppendButton.IsEnabled = true;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку добавления отдела
        /// </summary>
        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Department d = new Department();
            if (organization.FreeIDNumbers.Count != 0)
            {
                d.DepartmentID = organization.FreeIDNumbers[0];
                organization.FreeIDNumbers.RemoveAt(0);
            }
            else
            {
                d.DepartmentID = ++organization.MaxID;
            }

            AppendDepartmentWindow appendDepartmentWindow = new AppendDepartmentWindow();
            appendDepartmentWindow.Dispatcher.Invoke(() =>
            {
                appendDepartmentWindow.organization = organization;
                appendDepartmentWindow.dept = d;
                appendDepartmentWindow.mainWindow = this;
            });
            appendDepartmentWindow.ShowDialog();


            // Сразу добавляем руководителя
            AppendWorkerWindow appendWorkerWindow = new AppendWorkerWindow();
            appendWorkerWindow.Dispatcher.Invoke(() =>
            {
                appendWorkerWindow.cbCategory.IsEnabled = false;
                appendWorkerWindow.cbCategory.Visibility = Visibility.Hidden;
                appendWorkerWindow.btnAdd.Content = "Добавить руководителя";
                appendWorkerWindow.dept = d;
            });

            appendWorkerWindow.Title = "Добавление руководителя";
            appendWorkerWindow.ShowDialog();

            appendWorkerWindow.worker.Category = "Начальник отдела";
            if (d.Workers == null) d.Workers = new List<Worker>();
            if (appendWorkerWindow.worker != null) d.Workers.Add(appendWorkerWindow.worker);

            RefreshTreeView();
        }


        /// <summary>
        /// Метод обновления древовидного элемента
        /// </summary>
        public void RefreshTreeView()
        {
            // Пересчитать зарплату
            Department.Recount(organization);

            OrganizationView.Items.Clear();
            TreeViewItem item = new TreeViewItem();
            item.Header = organization.Name;
            GetSubDepartments(organization, item);
            OrganizationView.Items.Add(item);

            TreeViewItem treeViewItem = null;
            foreach (var ovi in OrganizationView.Items)
            {
                treeViewItem = ovi as TreeViewItem;
                treeViewItem.IsExpanded = true;
            }
        }

        /// <summary>
        /// Обработчик события закрытия окна - сохранение данных в файл
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string json;

            using (StreamWriter sw = new StreamWriter("Organization.json", false, Encoding.UTF8))
            {
                json = JsonConvert.SerializeObject(organization);
                sw.WriteLine(json);
            }
        }
    }
}
