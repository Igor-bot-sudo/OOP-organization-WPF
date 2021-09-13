using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace OrganizationStructure
{
    /// <summary>
    /// Логика взаимодействия для AppendWorkerWindow.xaml
    /// </summary>
    public partial class AppendWorkerWindow : Window
    {
        public Department dept;
        public Worker worker;

        public AppendWorkerWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbxFirstName.Text == "" || tbxLastName.Text == "" || tbxAge.Text == "")
            {
                MessageBox.Show("Нужно заполнить все поля ввода!");
                return;
            }

            Regex regex = new Regex(@"[^\d]");
            if (regex.IsMatch(tbxAge.Text))
            {
                MessageBox.Show("В поле возраст нужно ввести число!");
                return;
            }

            worker = new Worker();
            worker.FirstName = tbxFirstName.Text;
            worker.LastName = tbxLastName.Text;
            worker.Age = Int32.Parse(tbxAge.Text);

            ComboBoxItem selectedItem = (ComboBoxItem)cbCategory.SelectedItem;
            worker.Category = selectedItem.Content.ToString();

            worker.DepartmentID = dept.DepartmentID;

            if (worker.Category == "Кодер")
            {
                Coder c = new Coder();
                c.SetSalary(dept);
                worker.Salary = c.Salary;
            }
            else if (worker.Category == "Интерн")
            {
                Intern i = new Intern();
                i.SetSalary(dept);
                worker.Salary = i.Salary;
            } 
            
            Close();
        }
    }
}
