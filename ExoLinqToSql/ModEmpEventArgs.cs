using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExoLinqToSql
{
    public class ModEmpEventArgs : EventArgs
    {
        public Employee modEmp { get; private set; }

        public ModEmpEventArgs(Employee modEmp)
        {
            this.modEmp = modEmp;
        }
    }
}
