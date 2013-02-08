using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ExoLinqToSql
{
    public partial class Employee
    {
        NorthwindDataContext ndc = new NorthwindDataContext();

        public string FullName { get { return this._FirstName + " " + this._LastName; } }
        public string SupName { get { return GetSupName(); } }

        private string GetSupName()
        {
            string SupName = ndc.Employees.Where(e => e.EmployeeID == this.ReportsTo).Select(e => e.FullName).FirstOrDefault();
            return (SupName != null) ? SupName : "N/A";
        }
    }
}
