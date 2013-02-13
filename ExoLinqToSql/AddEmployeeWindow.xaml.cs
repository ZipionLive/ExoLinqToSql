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
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public event EventHandler<AddEmpEventArgs> AddEmployeeEvent;
        private MainWindow mWindow;

        public AddEmployeeWindow(MainWindow mWindow)
        {
            this.mWindow = mWindow;
            this.Register(this.mWindow);
            InitializeComponent();
            this.Show();
        }

        private void btnAddEmp_Click(object sender, RoutedEventArgs e)
        {
            if (AddEmployeeEvent != null)
            {
                try
                {
                    Employee newEmp = new Employee()
                    {
                        FirstName = tbFirstName.Text,
                        LastName = tbLastName.Text,
                        BirthDate = dpBirthDate.SelectedDate,
                        HireDate = DateTime.Now,
                        Address = tbAdress.Text + "\n" + tbAdress2.Text,
                        PostalCode = tbPostCode.Text,
                        City = tbCity.Text,
                        Country = tbCountry.Text,
                        Region = tbRegion.Text,
                    };

                    if (tbReportsTo.Text != string.Empty)
                    {
                        int rtID;
                        if (int.TryParse(tbReportsTo.Text, out rtID))
                            newEmp.ReportsTo = rtID;
                        else
                            throw new InvalidCastException("Le champ \"Supérieur\" doit contenir un nombre entier ou être vide");
                    }

                    AddEmployeeEvent(this, new AddEmpEventArgs(newEmp));
                    //MessageBox.Show("Envoyé : " + newEmp.FullName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Aucun abonné à \"AddEmployeeEvent\"");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbFirstName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            tbAdress.Text = string.Empty;
            tbAdress2.Text = string.Empty;
            tbPostCode.Text = string.Empty;
            tbCity.Text = string.Empty;
            tbCountry.Text = string.Empty;
            tbRegion.Text = string.Empty;
            tbReportsTo.Text = string.Empty;
            dpBirthDate.SelectedDate = DateTime.Now;
        }

        private void Register(MainWindow mWindow)
        {
            AddEmployeeEvent += new EventHandler<AddEmpEventArgs>(this.mWindow.HandleAddEmp);
        }

        private void Unregister(MainWindow mWindow)
        {
            AddEmployeeEvent -= this.mWindow.HandleAddEmp;
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Unregister(this.mWindow);
            this.Close();
        }
    }
}
