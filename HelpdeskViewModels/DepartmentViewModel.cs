using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private readonly DepartmentDAO _dao;
        

        public string? Timer { get; set; }     
        public string? Name { get; set; }
        public int? Id { get; set; }
        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        //
        // Retrieve all the employees as ViewModel instances
        //
        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allVms = new();
            try
            {
                List<Department> allDepartments = await _dao.GetAll();
                // we need to convert Department instance to EmployeeViewModel because
                // the Web Layer isn't aware of the Domain class Employee
                foreach (Department dep in allDepartments)
                {
                    DepartmentViewModel depVm = new()
                    {
                        Name = dep.DepartmentName,
                        Id = dep.Id,             
                        // binary value needs to be stored on client as base64
                        Timer = Convert.ToBase64String(dep.Timer!)
                    };
                    allVms.Add(depVm);
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

    

    }//end of class
}