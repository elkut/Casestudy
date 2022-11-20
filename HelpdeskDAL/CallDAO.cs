using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskDAL
{
    public class CallDAO
    {
        readonly IRepository<Call> _repo;
        public CallDAO()
        {
            _repo = new HelpdeskRepository<Call>();
        }

        public async Task<Call> GetByNote(string? note)
        {
            Call? selectedNote;
            try
            {
                selectedNote = await _repo.GetOne(call => call.Notes == note);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedNote!;
        }

        public async Task<int> Add(Call newCall)
        {
            try
            {
                HelpdeskContext _db = new();
                await _db.Calls.AddAsync(newCall);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return newCall.Id;
        }

        public async Task<Call> GetById(int id)
        {
            Call? selectedCall;
            try
            {
                selectedCall = await _repo.GetOne(call => call.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return selectedCall!;
        }

        public async Task<List<Call>> GetAll()
        {
            List<Call> allCalls;
            try
            {
                HelpdeskContext _db = new();
                allCalls = await _db.Calls.ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allCalls;
        }

        public async Task<UpdateStatus> Update(Call updatedCall)
        {
            UpdateStatus status;

            try
            {
                status = await _repo.Update(updatedCall);
            }
            catch (DbUpdateConcurrencyException)
            {
                status = UpdateStatus.Stale;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return status;
        }

        public async Task<int> Delete(int? id)
        {
            int CallDeleted = -1;
            try
            {
                CallDeleted = await _repo.Delete((int)id!); // returns # of rows removed

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return CallDeleted;
        }
    }
}
