using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        readonly IRepository<Department> _repo;
        public DepartmentDAO()
        {
            _repo = new HelpdeskRepository<Department>();
        }
        public async Task<List<Department>> GetAll()
        {
            List<Department> allDepartment;
            try
            {

                allDepartment = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allDepartment;
        }
    }
}
