using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrganizationStructure
{
    /// <summary>
    /// Логика взаимодействия для AppendDepartmentWindow.xaml
    /// </summary>
    public partial class AppendDepartmentWindow : Window
    {
        public Department organization;
        public Department dept;
        public MainWindow mainWindow;


        public AppendDepartmentWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbxDepartment.Text == "")
            {
                MessageBox.Show("Нужно указать название отдела!");
                return;
            }

            dept.Name = tbxDepartment.Text;
            dept.Workers = new List<Worker>();
            organization.Departments.Add(dept);
            Close();
            mainWindow.RefreshTreeView();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            lblDepartmentID.Content = "Департамент: ID = " + dept.DepartmentID.ToString();
        }
    }
}
