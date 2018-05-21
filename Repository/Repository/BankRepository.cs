using SharedLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    /// <summary>
    /// BankRepository
    /// </summary>

    public class BankRepository
    {
        public BankRepository()
        { }

        public virtual async Task<IEnumerable<Account>> GetAccounts()
        {
            using (BankContext context = new BankContext())
            {
                return await context.Account.ToListAsync();
            }
        }

        public virtual async Task<int> GetBalanceByAccountID(int accountID)
        {
            using (BankContext context = new BankContext())
            {
                var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                return account?.Balance ?? 0;
            }
        }

        public virtual async Task<bool> CreditAccount(int accountID, int amount)
        {
            try
            {
                using (BankContext context = new BankContext())
                {
                    var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                    account.Balance += amount;
                    context.Entry(account).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> DebitAccount(int accountID, int amount)
        {
            try
            {
                using (BankContext context = new BankContext())
                {
                    var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                    account.Balance -= amount;
                    context.Entry(account).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
