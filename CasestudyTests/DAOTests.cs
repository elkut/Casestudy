using HelpdeskDAL;
using ExercisesDAL;
using Xunit.Abstractions;

namespace CasestudyTests
{
    public class DAOTests
    {
        private readonly ITestOutputHelper output;
        public DAOTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task Call_ComprehensiveTest()
        {
            CallDAO cdao = new();
            EmployeeDAO edao = new();
            ProblemDAO pdao = new();
            Employee bigshot = await edao.GetByEmail("Alkut@abc.com");
            Employee burner = await edao.GetByEmail("Alkut@abc.com");
            Problem badDrive = await pdao.GetByDescription("Hard Drive Failure");
            Call call = new()
            {
                DateOpened = DateTime.Now,
                DateClosed = null,
                OpenStatus = true,
                EmployeeId = bigshot.Id,
                TechId = burner.Id,
                ProblemId = badDrive.Id,
                Notes = "Big shot's drive is shot, Alkut to fix it"
            };
            int newCallId = await cdao.Add(call);
            output.WriteLine("New Call Generated - Id = " + newCallId);
            call = await cdao.GetById(newCallId);
            byte[] oldtimer = call.Timer!;
            output.WriteLine("New Call Retrieved");
            call.Notes += "\n Ordered new drive!";
            if (await cdao.Update(call) == UpdateStatus.Ok)
            {
                output.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated!");
            }
            call.Timer = oldtimer;
            call.Notes = "doesn't matter data is stale now";
            if (await cdao.Update(call) == UpdateStatus.Stale)
            {
                output.WriteLine("Call was not updated due to stale data");
            }
            cdao = new CallDAO();
            await cdao.GetById(newCallId);
            if (await cdao.Delete(newCallId) == 1)
            {
                output.WriteLine("Call was deleted!");
            }
            else
            {
                output.WriteLine("Call was not deleted");
            }
            Assert.Null(await cdao.GetById(newCallId));
        }

        [Fact]
        public void Employee_GetByEmailTest()
        {
            HelpdeskContext _db = new();
            var selectedEmployees = from emp in _db.Employees
                                   where emp.Email == "sc@abc.com"
                                    select emp;
            Assert.True(selectedEmployees.Any());
        }

        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeDAO dao = new();
            Employee selectedEmployee = await dao.GetById(1);
            Assert.NotNull(selectedEmployee);
        }

        [Fact]
        public async Task Employee_GetAllTest()
        {
            EmployeeDAO dao = new();
            List<Employee> allEmployees = await dao.GetAll();
            Assert.True(allEmployees.Any());
        }

        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeDAO dao = new();
            Employee newEmployee = new()
            {
                FirstName = "Ailikuti",
                LastName = "Aisikaer",
                PhoneNo = "(555)555-1234",
                Title = "Mr.",
                DepartmentId = 100,
                Email = "some@abc.com"
            };
            Assert.True(await dao.Add(newEmployee) > 0);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeDAO dao = new();
            Employee? employeeForUpdate = await dao.GetByEmail("some@abc.com");
            if (employeeForUpdate != null)
            {
                string oldPhoneNo = employeeForUpdate.PhoneNo!;
                string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                employeeForUpdate!.PhoneNo = newPhoneNo;
            }
            Assert.True(await dao.Update(employeeForUpdate!) == UpdateStatus.Ok); // 1 indicates the # of rows updated
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeDAO dao = new();
            Employee? employeeForDelete = await dao.GetByEmail("bs@abc.com");
            Assert.True(await dao.Delete(employeeForDelete.Id) == 1); // 1 indicates the # of rows updated
        }

        [Fact]
        public async Task Employee_LoadPicsTest()
        {
            {
                CasestudyDALPicsUtil util = new();
                Assert.True(await util.AddEmployeePicsToDb());
            }
        }

        [Fact]
        public async Task Employee_ConcurrencyTest()
        {
            EmployeeDAO dao1 = new();
            EmployeeDAO dao2 = new();
            Employee employeeForUpdate1 = await dao1.GetByEmail("some@abc.com");
            Employee employeeForUpdate2 = await dao2.GetByEmail("some@abc.com");
            if (employeeForUpdate1 != null)
            {
                string? oldPhoneNo = employeeForUpdate1.PhoneNo;
                string? newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                employeeForUpdate1.PhoneNo = newPhoneNo;
                if (await dao1.Update(employeeForUpdate1) == UpdateStatus.Ok)
                {
                    // need to change the phone # to something else
                    employeeForUpdate2.PhoneNo = "666-666-6668";
                    Assert.True(await dao2.Update(employeeForUpdate2) == UpdateStatus.Stale);
                }
                else
                    Assert.True(false); // first update failed
            }
            else
                Assert.True(false); // didn't find student 1
        }
    }
}