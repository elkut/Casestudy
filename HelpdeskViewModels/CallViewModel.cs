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

        public async Task GetByNote()
        {
            try 
            {
                Call call = await _dao.GetByNote(Notes!);
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                EmployeeName = call.Employee.FirstName;
                ProblemDescription= call.Problem.Description;
                TechName = call.Tech.FirstName;
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
                Call call = await _dao.GetById(Id);
                Id = call.Id;
                DateOpened = call.DateOpened;
                if (!call.OpenStatus)
                {
                    DateClosed = call.DateClosed;
                }
                TechId = call.TechId;
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;
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
            List<CallViewModel> viewModels = new();
            try
            {
                List<Call> allCalls = await _dao.GetAll();
                ProblemDAO pdao = new();
                EmployeeDAO edao = new();

                    foreach (Call c in allCalls)
                    {
                        CallViewModel viewModel = new();
                        viewModel.Id = c.Id;
                        Problem p = await pdao.GetById(c.ProblemId);
                        viewModel.ProblemDescription = p.Description;
                        Employee e = await edao.GetById(c.EmployeeId);
                        viewModel.EmployeeName = e.LastName;
                        Employee t = await edao.GetById(c.TechId);
                        viewModel.OpenStatus = c.OpenStatus;
                        viewModel.TechName = t.LastName;
                        viewModel.DateClosed = c.DateClosed;
                        viewModel.DateOpened = c.DateOpened;
                        viewModel.Timer = Convert.ToBase64String(c.Timer!);
                        viewModel.EmployeeId = c.EmployeeId;
                        viewModel.ProblemId = c.ProblemId;
                        viewModel.TechId = c.TechId;
                        viewModel.Notes = c.Notes;
                        viewModels.Add(viewModel);
                    }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return viewModels;
        }

        public async Task Add()
        {
            //Id = -1;
            try
            {
                Call call = new()
                { 
                    EmployeeId = EmployeeId,
                   ProblemId = ProblemId,
                   TechId =TechId,
                   Notes = Notes!,
                   DateOpened = DateOpened,
                   OpenStatus = true,

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
                    Notes = Notes!,
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
            long rowsDeleted;
            try
            {
                // dao will return # of rows deleted
                // return await _dao.Delete(Id);
                rowsDeleted = await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return Convert.ToInt16(rowsDeleted);
        }
    }
}
