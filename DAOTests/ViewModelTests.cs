using Xunit;
using HelpdeskViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeViewModel vm = new() { Email = "td@abc.com" };
            await vm.GetByEmail();
            Assert.NotNull(vm.Firstname);
        }
        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeViewModel vm = new() { Id = 3 };
            await vm.GetById();
            Assert.NotNull(vm.Id);
        }
        [Fact]
        public static async Task Employee_GetAllTest()
        {
            List<EmployeeViewModel> allEmployeeVms;
            EmployeeViewModel vm = new();
            allEmployeeVms = await vm.GetAll();
            Assert.True(allEmployeeVms.Count > 0);
        }

        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeViewModel vm;
            vm = new()
            {
                Title = "Mr.",
                Firstname = "Shashwat",
                Lastname = "Malla",
                Email = "s_malla156680@fanshaweonline.ca",
                Phoneno = "(555)555-5551",
                DepartmentId = 100 // ensure division id is in Division table
            };
            await vm.Add();
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeViewModel vm = new() { Email = "some@abc.com" };
            await vm.GetByEmail(); // Student just added in Add test
            vm.Phoneno = vm.Phoneno == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
            // will be -1 if failed 0 if no data changed, 1 if succcessful
            Assert.True(await vm.Update() == 1);
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeViewModel vm = new() { Email = "some@abc.com" };
            await vm.GetByEmail(); // Student just added
            Assert.True(await vm.Delete() == 1); // 1 student deleted
        }
    }
}
