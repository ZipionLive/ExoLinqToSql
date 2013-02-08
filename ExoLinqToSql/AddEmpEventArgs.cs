using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExoLinqToSql
{
    public class AddEmpEventArgs : EventArgs
    {
        public Employee newEmp { get; set; }

        public AddEmpEventArgs(Employee e)
        {
            this.newEmp = e;
        }
    }
}
