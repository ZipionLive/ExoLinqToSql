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
    /// Interaction logic for DelEmployeeWindow.xaml
    /// </summary>
    public partial class DelEmployeeWindow : Window
    {
        public event EventHandler<DelEmpEventArgs> DelEmployeeEvent;
        private MainWindow mWindow;

        public DelEmployeeWindow(MainWindow mWindow)
        {
            this.mWindow = mWindow;
            this.Register(this.mWindow);
            InitializeComponent();
            this.Show();
        }

        private void Register(MainWindow mWindow)
        {
            DelEmployeeEvent += new EventHandler<DelEmpEventArgs>(this.mWindow.HandleDelEmp);
        }

        private void Unregister(MainWindow mWindow)
        {
            DelEmployeeEvent -= this.mWindow.HandleDelEmp;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int ID;

            if (tbID.Text != string.Empty && int.TryParse(tbID.Text, out ID))
            {
                DelEmployeeEvent(this, new DelEmpEventArgs(ID));
            }
            else { MessageBox.Show("Veuillez entrer un nombre entier"); }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Unregister(this.mWindow);
            this.Close();
        }
    }
}
