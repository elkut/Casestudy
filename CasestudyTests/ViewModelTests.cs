using Amazon.CloudFormation.Model;
using HelpdeskViewModels;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using MiNET.Plugins;
using Xunit.Abstractions;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        private readonly ITestOutputHelper output;
        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeViewModel vm = new() { Email = "td@abc.com" };
            await vm.GetByEmail();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Empoyee_GetByIdTest()
        {
            EmployeeViewModel vm = new() { Id = 2 };
            await vm.GetById();
            Assert.NotNull(vm.Firstname);
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
                Firstname = "Ailikuti",
                Lastname = "Aisikaer",
                Email = "some@abc.com",
                Phoneno = "(555)555-5551",
                DepartmentId = 100 
            };
            await vm.Add();
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeViewModel vm = new() { Email = "some@abc.com" };
            await vm.GetByEmail(); // Employee just added in Add test
            vm.Phoneno = vm.Phoneno == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
            // will be -1 if failed 0 if no data changed, 1 if succcessful
             Assert.True(await vm.Update() == 1);
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeViewModel vm = new() { Email = "some@abc.com" };
            await vm.GetByEmail(); // Employee just added
            Assert.True(await vm.Delete() == 1); // 1 Employee deleted
        }

        [Fact]
        public async Task Employee_ConcurrencyTest()
        {
            EmployeeViewModel vm1 = new() { Email = "some@abc.com" };
            EmployeeViewModel vm2 = new() { Email = "some@abc.com" };
            await vm1.GetByEmail(); // Fetch same student to simulate 2 users
            if (vm1.Email != "Not Found") // make sure we found a student
            {
                await vm2.GetByEmail(); // fetch same data
                vm1.Phoneno = vm1.Phoneno == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
                if (await vm1.Update() == 1)
                {
                    vm2.Phoneno = "(666)666-6666"; // just need any value
                    Assert.True(await vm2.Update() == -2);
                }
            }
            else
            {
                Assert.True(false); // student not found
            }
        }

        [Fact]
        public async Task Call_ComprehensiveVMTest()
        {
            CallViewModel cvm = new();
            EmployeeViewModel evm = new();
            ProblemViewModel pvm = new();
            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Email = "Alkut@abc.com";
            await evm.GetByEmail();
            cvm.EmployeeId = Convert.ToInt16(evm.Id);
            evm.Email = "Alkut.Askar@abc.com";
            await evm.GetByEmail();
            cvm.TechId = Convert.ToInt16(evm.Id);
            pvm.Description = "Memory Upgrade";
            await pvm.GetByDescription();
            cvm.ProblemId = (int)pvm.Id;
            cvm.Notes = "Bigshot has bad RAM, Burner to fix it";
            await cvm.Add();
            output.WriteLine("New Call Generated - Id = " + cvm.Id);
            int id = cvm.Id; // need id for delete later
            await cvm.GetById();
            cvm.Notes += "\n Ordered new RAM!";
            if (await cvm.Update() == 1)
            {
                output.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated!");
            }
            cvm.Notes = "Another change to comments that should not work";
            if (await cvm.Update() == -2)
            {
                output.WriteLine("Call was not updated data was stale");
            }
            cvm = new CallViewModel
            {
                Id = id
            };
            // need to reset because of concurrency error
            await cvm.GetById();
            if (await cvm.Delete() == 1)
            {
                output.WriteLine("Call was deleted!");
            }
            else
            {
                output.WriteLine("Call was not deleted");
            }
            // should throw expected exception
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async () => await cvm.GetById());
        }

    }
}
