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
        int eListInitCount;

        public MainWindow()
        {
            InitializeComponent();

            this.dgNorthwindEmp.ItemsSource = eList;
            
            LoadData();
        }

        /// <summary>
        /// Charge (ou re-charge) le contenu de la table Employees dans eList
        /// </summary>
        private void LoadData()
        {
            var employees = ndc.Employees.Select(e => e);

            eList.Clear();

            //OU

            //for (int i = eList.Count; i > 0; i--)
            //{
            //    eList.RemoveAt(0);
            //}

            foreach (var e in employees)
            {
                eList.Add(e);
            }

            eListInitCount = eList.Count();
        }

        /// <summary>
        /// Ajoute les employés créés par la fenêtre "AddEmployeeWindow" à eList
        /// </summary>
        /// <param name="sender">AddEmployeeWindow</param>
        /// <param name="e">un objet de type AddEmpEventArgs contenant un objet Employee</param>
        public void HandleAddEmp(object sender, AddEmpEventArgs args)
        {
            args.newEmp.EmployeeID = eList.Select(emp => emp.EmployeeID).Max() + 1;
            eList.Add(args.newEmp);
            //this.dgNorthwindEmp.ItemsSource = eList;
        }

        public void HandleDelEmp(object sender, DelEmpEventArgs args)
        {
            if (eList.Select(e => e.EmployeeID).ToList().Contains(args.delEmpID))
            {
                Employee emp = eList.Where(e => e.EmployeeID == args.delEmpID).SingleOrDefault();
                eList.Remove(emp);
            }
            else { MessageBox.Show("Il n'existe aucun employé portant l'ID \"" + args.delEmpID + "\" !"); }
        }

        /// <summary>
        /// Ouvre une fenêtre "AddEmployeeWindow" en réponse à un évènement de clic sur le bouton "btnAdd"
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmpWindow = new AddEmployeeWindow(this);
        }

        /// <summary>
        /// Répercute sur la DB les modifications faites dans eList
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sbChanges = new StringBuilder("Changements effectués :\n");
            List<int> employeeIDs = eList.Select(emp => emp.EmployeeID).ToList();
            List<Employee> newEmployees = eList.Where(emp => emp.EmployeeID >= eListInitCount + 1).Select(emp => emp).ToList();
            List<Employee> deletedEmployees = ndc.Employees.Where(emp => !employeeIDs.Contains(emp.EmployeeID)).ToList();
            if (newEmployees.Count() != 0)
            {
                ndc.Employees.InsertAllOnSubmit(newEmployees);
                ndc.SubmitChanges();

                sbChanges.AppendLine("\nEmployés ajoutés :");
                foreach (Employee newEmp in newEmployees)
                    sbChanges.AppendLine(newEmp.FullName);
            }

            if (deletedEmployees.Count != 0)
            {
                ndc.Employees.DeleteAllOnSubmit(deletedEmployees);
                ndc.SubmitChanges();

                sbChanges.AppendLine("\nEmployés supprimés :");
                foreach (Employee delEmp in deletedEmployees)
                    sbChanges.AppendLine(delEmp.FullName);
            }

            MessageBox.Show(sbChanges.ToString());
            LoadData();
        }

        /// <summary>
        /// Annule les modifications apportées à eList depuis la dernière sauvegarde
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSuppr_Click(object sender, RoutedEventArgs e)
        {
            DelEmployeeWindow delEmpWindow = new DelEmployeeWindow(this);
        }
    }
}
