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
using System.Windows.Shapes;

namespace ExoLinqToSql
{
    /// <summary>
    /// Interaction logic for ModEmployee.xaml
    /// </summary>
    public partial class ModEmployeeWindow : Window
    {
        public event EventHandler<ModEmpEventArgs> ModEmployeeEvent;

        private MainWindow mWindow;
        private List<int> IDList; 
        private Employee modEmployee;

        public ModEmployeeWindow(MainWindow mWindow, Employee oldEmployee, List<int> IDList)
        {
            this.mWindow = mWindow;
            this.modEmployee = new Employee()
            {
                EmployeeID = oldEmployee.EmployeeID,
                FirstName = oldEmployee.FirstName,
                LastName = oldEmployee.LastName,
                BirthDate = oldEmployee.BirthDate,
                HireDate = oldEmployee.HireDate,
                Address = oldEmployee.Address,
                PostalCode = oldEmployee.PostalCode,
                City = oldEmployee.City,
                Country = oldEmployee.Country,
                Region = oldEmployee.Region,
                ReportsTo = oldEmployee.ReportsTo,
                EmployeeTerritories = oldEmployee.EmployeeTerritories,
                Extension = oldEmployee.Extension,
                HomePhone = oldEmployee.HomePhone,
                Notes = oldEmployee.Notes,
                Photo = oldEmployee.Photo,
                PhotoPath = oldEmployee.PhotoPath,
                Title = oldEmployee.Title,
                TitleOfCourtesy = oldEmployee.TitleOfCourtesy
            };
            this.IDList = IDList;
            this.Register(this.mWindow);
            InitializeComponent();
            FillFieldsDefault();
            this.Show();
        }

        private void FillFieldsDefault()
        {
            tbFirstName.Text = modEmployee.FirstName;
            tbLastName.Text = modEmployee.LastName;
            tbAdress.Text = modEmployee.Address;
            tbAdress2.Text = string.Empty;
            tbPostCode.Text = modEmployee.PostalCode;
            tbCity.Text = modEmployee.City;
            tbCountry.Text = modEmployee.Country;
            tbRegion.Text = modEmployee.Region;
            tbReportsTo.Text = modEmployee.ReportsTo.ToString();
            dpBirthDate.SelectedDate = modEmployee.BirthDate;
        }

        private void btnModEmp_Click(object sender, RoutedEventArgs e)
        {
            if (ModEmployeeEvent != null)
            {
                try
                {
                    modEmployee.FirstName = tbFirstName.Text;
                    modEmployee.LastName = tbLastName.Text;
                    modEmployee.BirthDate = dpBirthDate.SelectedDate;
                    modEmployee.Address = tbAdress.Text + "\n" + tbAdress2.Text;
                    modEmployee.PostalCode = tbPostCode.Text;
                    modEmployee.City = tbCity.Text;
                    modEmployee.Country = tbCountry.Text;
                    modEmployee.Region = tbRegion.Text;

                    if (tbReportsTo.Text != string.Empty)
                    {
                        int rtID;
                        if (int.TryParse(tbReportsTo.Text, out rtID))
                        {
                            if (rtID != modEmployee.EmployeeID)
                                modEmployee.ReportsTo = rtID;
                            else
                            {
                                throw new InvalidOperationException("Un employé ne peut pas être son propre supérieur.\nSi l'employé n'a pas de supérieur hiérarchique, laissez le champ \"ID du Supérieur\" vide.");
                            }
                        }
                        else
                            throw new InvalidCastException("Le champ \"Supérieur\" doit contenir un nombre entier ou être vide");
                    }

                    ModEmployeeEvent(this, new ModEmpEventArgs(modEmployee));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Aucun abonné à \"ModEmployeeEvent\"");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            FillFieldsDefault();
        }

        private void Register(MainWindow mWindow)
        {
            ModEmployeeEvent += new EventHandler<ModEmpEventArgs>(this.mWindow.HandleModEmp);
        }

        private void Unregister(MainWindow mWindow)
        {
            ModEmployeeEvent -= this.mWindow.HandleModEmp;
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Unregister(this.mWindow);
            this.Close();
        }
    }
}
