using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OrganizationStructure
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Department organization;
        public Department dept;
        public TreeView tv;
        public MainWindow mainWindow;

        public EditWindow()
        {
            InitializeComponent();
        }


        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        string sortBy;

        /// <summary>
        /// Обработчик клика на заголовке табличного списка для сортировки
        /// </summary>
        void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                            Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        // Сортировка данных по выбранному параметру
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lvWorkers.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            lblDepartment.Content = "Департамент: " + dept.Name + ", ID = " + dept.DepartmentID;
            lvWorkers.ItemsSource = dept.Workers;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления отдела
        /// </summary>
        private void DeleteDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            organization.FreeIDNumbers.Add(dept.DepartmentID);
            Department.DeleteDepartment(organization, dept);
            Close();
            mainWindow.RefreshTreeView();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку удаления данных сотрудника
        /// </summary>
        private void DeleteWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbxCategory.Text == "Начальник отдела")
            {
                MessageBox.Show("Удалить данные начальника отдела можно только вместе с отделом!");
                return;
            }

            Worker w = new Worker();

            w.FirstName = tbxFirstName.Text;
            w.LastName = tbxLastName.Text;
            w.DepartmentID = dept.DepartmentID;
            Department.DeleteWorker(dept, w);

            lvWorkers.RaiseEvent(new RoutedEventArgs(GridViewColumnHeader.ClickEvent, _lastHeaderClicked));
            lvWorkers.RaiseEvent(new RoutedEventArgs(GridViewColumnHeader.ClickEvent, _lastHeaderClicked));

            mainWindow.RefreshTreeView();
            lvWorkers.Items.Refresh();
        }


        /// <summary>
        /// Обработчик нажатия на кнопку добавления сотрудника
        /// </summary>
        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            AppendWorkerWindow appendWorkerWindow = new AppendWorkerWindow();

            appendWorkerWindow.Dispatcher.Invoke(() =>
            {
                appendWorkerWindow.dept = dept;
            });

            appendWorkerWindow.ShowDialog();

            if (dept.Workers == null) dept.Workers = new List<Worker>();
            if (appendWorkerWindow.worker != null) dept.Workers.Add(appendWorkerWindow.worker);

            lvWorkers.RaiseEvent(new RoutedEventArgs(GridViewColumnHeader.ClickEvent, _lastHeaderClicked));
            lvWorkers.RaiseEvent(new RoutedEventArgs(GridViewColumnHeader.ClickEvent, _lastHeaderClicked));

            mainWindow.RefreshTreeView();
            lvWorkers.Items.Refresh();
        }


        /// <summary>
        /// Обработчик нажатия на кнопку обновления данных сотрудника
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"[^\d]");
            if (regex.IsMatch(tbxAge.Text))
            {
                MessageBox.Show("В поле возраст нужно ввести число!");
                return;
            }

            mainWindow.RefreshTreeView();
            lvWorkers.Items.Refresh();
        }
    }
}
