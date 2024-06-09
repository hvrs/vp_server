using Microsoft.EntityFrameworkCore;
using vp_server.Models;
namespace vp_server.Utils
{
    public class deleleteTransactions
    {
        public async void delete()
        {
            using (VapeshopContext db = new VapeshopContext())
            {
                RemoveTime? rt = await db.RemoveTimes.FirstOrDefaultAsync();
                if (rt != null)
                {
                    List<Transaction> transactionsToDelete = await db.Transactions.Where(t=> (DateOnly.FromDateTime(DateTime.Now).DayNumber - t.Date.DayNumber) >= rt.DaysToRemove).ToListAsync();
                    if (transactionsToDelete != null)
                    {
                        foreach (Transaction transaction in transactionsToDelete)
                        {
                            db.Transactions.Remove(transaction);
                            await db.SaveChangesAsync();
                        }
                    }                    
                }
            }
        }
    }
}
