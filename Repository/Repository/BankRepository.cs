using SharedLib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

        public virtual async Task<TransactionRecord> CreditAccount(int accountID, int amount)
        {
            int lastBalance = 0;
            TransactionRecord transactionRecord;
            try
            {
                using (BankContext context = new BankContext())
                {
                    //using queries
                    var sqlQuery = $"Update Account set Balance = Balance + {amount} where AccountID = {accountID}";
                    var result = await context.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction, sqlQuery);
                    var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                    lastBalance = account.Balance;
                    transactionRecord = new TransactionRecord()
                    {
                        OperationType = "Credit",
                        TransactionAmount = amount,
                        TransactionDate = DateTime.Now,
                        Balance = account.Balance,
                        IsSuccess = result > 0
                    };

                    //using API's
                    //var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                    //account.Balance += amount;
                    //context.Entry(account).State = EntityState.Modified;
                    //var result = await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                transactionRecord = new TransactionRecord()
                {
                    OperationType = "Credit",
                    TransactionAmount = amount,
                    TransactionDate = DateTime.Now,
                    Balance = lastBalance,
                    IsSuccess = false
                };
            }
            return transactionRecord;

        }

        public virtual async Task<TransactionRecord> DebitAccount(int accountID, int amount)
        {
            int lastBalance = 0;
            TransactionRecord transactionRecord;
            try
            {
                //Test(accountID, amount);
                //return true;
                using (BankContext context = new BankContext())
                {
                    //using queries
                    var updateBalanceSqlQuery = $"Update Account set Balance = Balance -{amount} where AccountID = {accountID}";
                    var result = await context.Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction, updateBalanceSqlQuery);
                    var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                    lastBalance = account.Balance;
                    transactionRecord = new TransactionRecord()
                    {
                        OperationType = "Debit",
                        TransactionAmount = amount,
                        TransactionDate = DateTime.Now,
                        Balance = account.Balance,
                        IsSuccess = result > 0
                    };

                    //using API's
                    //var account = await context.Account.FirstOrDefaultAsync(x => x.AccountID == accountID);
                    //account.Balance -= amount;
                    //context.Entry(account).State = EntityState.Modified;
                    //var result = await context.SaveChangesAsync();
                    return transactionRecord;
                }
            }
            catch (Exception ex)
            {
                transactionRecord = new TransactionRecord()
                {
                    OperationType = "Debit",
                    TransactionAmount = amount,
                    TransactionDate = DateTime.Now,
                    Balance = lastBalance,
                    IsSuccess = false
                };
            }
            return transactionRecord;
        }

        /// <summary>
        /// Sample code to try sql query update statement
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="amount"></param>
        void Test(int accountID, int amount)
        {
            SqlConnection cn = new SqlConnection();
            SqlCommand DAUpdateCmd;
            cn.ConnectionString = "Server=localhost;Database=Bank;Integrated Security=True;MultipleActiveResultSets=True";
            cn.Open();
            DAUpdateCmd = new SqlCommand($"Update Account set Balance = Balance -{amount} where AccountID = {accountID}", cn);
            DAUpdateCmd.ExecuteNonQuery();
            cn.Close();
        }
    }
}
