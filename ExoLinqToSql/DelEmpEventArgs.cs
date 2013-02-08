using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExoLinqToSql
{
    public class DelEmpEventArgs : EventArgs
    {
        public int delEmpID { get; private set; }

        public DelEmpEventArgs(int ID)
        {
            this.delEmpID = ID;
        }
    }
}
