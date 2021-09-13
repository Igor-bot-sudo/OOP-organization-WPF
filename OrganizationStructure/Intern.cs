using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationStructure
{
    public class Intern : Worker
    {
        public override int  SetSalary(Department d)
        {
            this.Salary = 500;
            return 0;
        }
    }
}
