
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        readonly IRepository<Problem> _repo;
        public ProblemDAO()
        {
            _repo = new HelpdeskRepository<Problem>();
        }

        public async Task<List<Problem>> GetAll()
        {
            List<Problem> allProblems;
            try
            {
                HelpdeskContext _db = new();
                allProblems = await _db.Problems.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allProblems;
        }

        public async Task<Problem> GetById(int id)
        {
            Problem? selectedProblem;
            try
            {
                selectedProblem = await _repo.GetOne(prob => prob.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedProblem!;
        }

        public async Task<Problem> GetByDescription(string? description)
        {
            Problem? selectedProblem;
            try
            {
                selectedProblem = await _repo.GetOne(emp => emp.Description == description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedProblem!;
        }

    }
}
