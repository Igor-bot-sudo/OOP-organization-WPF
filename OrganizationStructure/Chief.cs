namespace OrganizationStructure
{
    public class Chief : Worker
    {
        public override int SetSalary(Department dept)
        {
            int result = 0;
            int sum = 0;

            if (dept.Departments != null)
            {
                foreach (var d in dept.Departments)
                {
                    if (d.Workers != null)
                    {
                        foreach (var w in d.Workers)
                        {
                            if (w.Category == "Начальник отдела")
                            {
                                Manager m = new Manager();
                                result = m.SetSalary(d);
                                w.Salary = m.Salary;
                                sum += result;
                                break;
                            }
                        }
                    }
                }

                result = sum;
                sum = (int)(sum * 0.15f);
                if (sum < 1300) sum = 1300;
                this.Salary = sum;
                result += this.Salary;
            }

            return result;
        }
    }
}
