﻿using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;
        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? Id { get; set; }
        public bool? IsTech { get; set; }
        public string? StaffPicture64 { get; set; }
        // constructor
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        public async Task GetByEmail()
        {
            try
            {
                Employee emp = await _dao.GetByEmail(Email!);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                 MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task GetById()
        {
            try
            {
                Employee emp = await _dao.GetById((int)Id!);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                 MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
    
        //
        // Retrieve all the employees as ViewModel instances
        //
        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new();
            try
            {
                List<Employee> allEmployees = await _dao.GetAll();
                // we need to convert Employee instance to EmployeeViewModel because
                // the Web Layer isn't aware of the Domain class Employee
                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new()
                    {
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        DepartmentName = emp.Department.DepartmentName,
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

        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId
                };
                Id = await _dao.Add(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    Id = (int)Id!,
                    DepartmentId = DepartmentId
                };
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                emp.Timer = Convert.FromBase64String(Timer!);
                updateStatus = -1;
                updateStatus = (int)await _dao.Update(emp); // overwrite status
                }

            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }
        public async Task<int> Delete()
        {
            try
            {
                // dao will return # of rows deleted
                return await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

    }//end of class
}