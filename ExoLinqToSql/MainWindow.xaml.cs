using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace ExoLinqToSql
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NorthwindDataContext ndc = new NorthwindDataContext();
        ObservableCollection<Employee> eList = new ObservableCollection<Employee>();

        public MainWindow()
        {
            InitializeComponent();

            this.dgNorthwindEmp.ItemsSource = eList;
            
            LoadData();
        }

        private void LoadData()
        {
            var employees = ndc.Employees.Select(e => e);

            foreach (var e in employees)
            {
                eList.Add(e);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmpWindow = new AddEmployeeWindow(this);
        }

        public void HandleAddEmp(object sender, AddEmpEventArgs e)
        {
            e.newEmp.EmployeeID = eList.Select(emp => emp.EmployeeID).Max() + 1;
            eList.Add(e.newEmp);
            this.dgNorthwindEmp.ItemsSource = eList;
            //MessageBox.Show("Ajouté : " + eList.ElementAt(eList.Count() - 1).FullName);
        }
    }
}
