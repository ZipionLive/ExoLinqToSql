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
        List<int> modEmpIDs = new List<int>();

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
        /// <param name="e">Un objet de type AddEmpEventArgs contenant un objet Employee</param>
        public void HandleAddEmp(object sender, AddEmpEventArgs args)
        {
            args.newEmp.EmployeeID = eList.Select(emp => emp.EmployeeID).Max() + 1;
            eList.Add(args.newEmp);
            //this.dgNorthwindEmp.ItemsSource = eList;
        }

        /// <summary>
        /// Supprime de eList l'employé sélectionné dans la fenêtre "DelEmployeeWindow"
        /// </summary>
        /// <param name="sender">DelEmployeeWindow</param>
        /// <param name="args">L'ID de l'employé à supprimer</param>
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
        /// Met à jour un employé avec les valeurs entrées dans la fenêtre "ModEmployeeWindow"
        /// </summary>
        /// <param name="sender">ModEmployeeWindow</param>
        /// <param name="args">Un objet de type AddEmpEventArgs contenant un objet Employee</param>
        public void HandleModEmp(object sender, ModEmpEventArgs args)
        {
            try
            {
                Employee modEmp = eList.Where(e => e.EmployeeID == args.modEmp.EmployeeID).SingleOrDefault();
                int modEmpIndex = eList.IndexOf(modEmp);
                eList[modEmpIndex] = args.modEmp;
                modEmpIDs.Add(args.modEmp.EmployeeID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        private void btnSave_Click(object sender, RoutedEventArgs args)
        {
            StringBuilder sbChanges = new StringBuilder("Changements effectués :\n");
            List<int> employeeIDs = eList.Select(e => e.EmployeeID).ToList();
            List<Employee> newEmployees = eList.Where(e => e.EmployeeID >= eListInitCount + 1).Select(emp => emp).ToList();
            List<Employee> deletedEmployees = ndc.Employees.Where(e => !employeeIDs.Contains(e.EmployeeID)).ToList();
            List<Employee> outdatedEmployees = ndc.Employees.Where(e => modEmpIDs.Contains(e.EmployeeID)).ToList();
            List<Employee> updatedEmployees = eList.Where(e => modEmpIDs.Contains(e.EmployeeID)).ToList();
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

            if (outdatedEmployees.Count() != 0)
            {
                ndc.Employees.DeleteAllOnSubmit(outdatedEmployees);
                ndc.SubmitChanges();

                ndc.Employees.InsertAllOnSubmit(updatedEmployees);
                ndc.SubmitChanges();

                sbChanges.AppendLine("\nEmployés modifiés :");
                foreach (Employee modEmp in updatedEmployees)
                    sbChanges.AppendLine(modEmp.FullName);
            }

            MessageBox.Show(sbChanges.ToString());
            modEmpIDs.Clear();
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

        /// <summary>
        /// Ouvre une fenêtre "DelEmployeeWindow" en réponse à un évènement de clic sur le bouton "btnSuppr"
        /// </summary>
        private void btnSuppr_Click(object sender, RoutedEventArgs e)
        {
            DelEmployeeWindow delEmpWindow = new DelEmployeeWindow(this);
        }

        /// <summary>
        /// Ouvre une fenêtre "ModEmployeeWindow" en réponse à un évènement de clic sur le bouton "modAdd"
        /// </summary>
        private void btnMod_Click(object sender, RoutedEventArgs args)
        {
            try
            {
                List<int> IDList = eList.Select(e => e.EmployeeID).ToList();
                int selectIndex = dgNorthwindEmp.SelectedIndex;
                Employee modEmp = ndc.Employees.ToList().ElementAt(selectIndex);
                ModEmployeeWindow modEmpWindow = new ModEmployeeWindow(this, modEmp, IDList);
            }
            catch
            {
                MessageBox.Show("Veuillez d'abord sélectionner un employé dans la liste.");
            }
        }
    }
}
