using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationStructure
{
    public class Coder : Worker
    {
        public override int SetSalary(Department d)
        {
            this.Salary = 12 * 8 * 22;
            return 0;
        }
    }
}
