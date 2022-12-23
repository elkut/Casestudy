
using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;


namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private readonly ProblemDAO _dao;
        public string? Description { get; set; }
        public int? Id { get; set; }

     
        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        public async Task GetByDescription()
        {
            try
            {
                Problem pro = await _dao.GetByDescription(Description);
                Description = pro.Description;
                Id = pro.Id;
             
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }


        public async Task<List<ProblemViewModel>> GetAll()
        {
            List<ProblemViewModel> allVms = new();
            try
            {
                List<Problem> allProblems = await _dao.GetAll();
             
                foreach (Problem pro in allProblems)
                {
                    ProblemViewModel proVm = new()
                    {
                        Id = pro.Id,
                        Description = pro.Description,
                     
                    };
                  
                    allVms.Add(proVm);
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
