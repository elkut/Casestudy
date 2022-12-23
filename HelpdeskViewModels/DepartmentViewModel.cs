using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        readonly private DepartmentDAO _dao;
        public string? Timer { get; set; }
        public string? Name { get; set; }
        public int? Id { get; set; }
        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allVms = new();
            try
            {
                List<Department> allEmployees = await _dao.GetAll();
                // we need to convert Employee instance to EmployeeViewModel because
                // the Web Layer isn't aware of the Domain class Employee
                foreach (Department emp in allEmployees)
                {
                    DepartmentViewModel empVm = new()
                    {
                        Name = emp.DepartmentName,
                        Id = emp.Id,
                        // binary value needs to be stored on client as base64
                        Timer = Convert.ToBase64String(emp.Timer!)
                    };
                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }
    }
}

