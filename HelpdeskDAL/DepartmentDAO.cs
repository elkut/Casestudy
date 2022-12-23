using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;


namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        public async Task<List<Department>> GetAll()
        {
            List<Department> allEmployees;
            try
            {
                HelpdeskContext _db = new();
                allEmployees = await _db.Departments.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allEmployees;
        }
    }
}