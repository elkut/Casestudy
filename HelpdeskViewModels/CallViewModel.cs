using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private readonly CallDAO _dao;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ProblemDescription { get; set; }
        public string? TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string? Notes { get; set; }
        public string? Timer { get; set; }

        public CallViewModel()
        {
            _dao = new CallDAO();
        }

        public async Task GetByNotes()
        {
            try 
            {
                Call call = await _dao.GetByNote(Notes!);
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                ProblemDescription= call.Problem.Description;
                TechName = call.Employee.LastName;
                TechId = call.TechId;
                Id = call.Id;
                DateTime dateOpened = call.DateOpened;
                DateTime? dateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Timer = Convert.ToBase64String(call.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Notes = "not found";
            }
            catch (Exception ex)
            {
                Notes = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task GetById()
        {
            try
            {
                Call call = await _dao.GetById((int)Id!);
                Id = call.Id;
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                ProblemDescription = call.Problem.Description;
                TechName = call.Employee.LastName;
                TechId = call.TechId;
                Id = call.Id;
                DateTime dateOpened = call.DateOpened;
                DateTime? dateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Timer = Convert.ToBase64String(call.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<List<CallViewModel>> GetAll()
        {
            List<CallViewModel> allVms = new();
            try
            {
                List<Call> allCalls = await _dao.GetAll();
                // we need to convert Student instance to StudentViewModel because
                // the Web Layer isn't aware of the Domain class Student
                foreach (Call call in allCalls)
                {
                    CallViewModel callVm = new()
                    {
                        Id = call.Id,
                        EmployeeId = call.EmployeeId,
                        ProblemId = call.ProblemId,
                        EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName,
                        ProblemDescription = call.Problem.Description,
                        TechName = call.Tech.FirstName,
                        TechId = call.TechId,
                        DateOpened = call.DateOpened,
                        DateClosed = call.DateClosed,
                        OpenStatus = call.OpenStatus,

                    Timer = Convert.ToBase64String(call.Timer!),
                    };

                    allVms.Add(callVm);
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
                Call call = new()
                {
                    EmployeeId = EmployeeId,
                   ProblemId = ProblemId,
                   TechId =TechId,
                   Notes = Notes,
                   DateOpened = DateOpened,
                   DateClosed = DateClosed,
                   OpenStatus = OpenStatus,

                };
                Id = await _dao.Add(call);
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

            try
            {
                Call call = new()
                {
                    EmployeeId = EmployeeId,
                    ProblemId = ProblemId,
                    TechId = TechId,
                    Notes = Notes,
                    DateOpened = DateOpened,
                    DateClosed = DateClosed,
                    OpenStatus = OpenStatus,
                    Id = (int)Id!,
                   
                };
                
                call.Timer = Convert.FromBase64String(Timer!);
                // dao will pass UpdateStatus of 1, -1, or -2
                return Convert.ToInt16(await _dao.Update(call));

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
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
    }
}
