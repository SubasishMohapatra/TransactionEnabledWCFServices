using SharedLib;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.ServiceModel;
using Repository;
using System.Threading.Tasks;
//TransactionIsolationLevel =
//    System.Transactions.IsolationLevel.ReadUncommitted,
//  TransactionTimeout = "00:10:30",
//  InstanceContextMode = InstanceContextMode.PerSession,
//  TransactionAutoCompleteOnSessionClose = true)]
namespace BankingService
{
    [ServiceBehavior(
  TransactionIsolationLevel =
    System.Transactions.IsolationLevel.ReadCommitted)]

    public class AccountService : IAccountService
    {
        BankRepository bankRepository = new BankRepository();
        CouchbaseRepository couchbaseRepository = new CouchbaseRepository();

        public AccountService()
        {
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Task<bool> Credit(int accountID, int amount)
        {
            var creditAccountTask = Task.Run(async () =>
               {
                   return await bankRepository.CreditAccount(accountID, amount);
               });
            var result = creditAccountTask.ContinueWith(async (t) =>
              {
                  return t.Result != null ? await couchbaseRepository.CreditAccount(accountID, t.Result) : false;
              }, TaskContinuationOptions.OnlyOnRanToCompletion);
            return result.Result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Task<bool> Debit(int accountID, int amount)
        {
            var debitAccountTask = Task.Run(async () =>
            {
                return await bankRepository.DebitAccount(accountID, amount);
            });
            var result = debitAccountTask.ContinueWith(async (t) =>
                {
                    return t.Result != null ? await couchbaseRepository.DebitAccount(accountID, t.Result) : false;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);
            return result.Result;
        }

        public Task<IEnumerable<Account>> GetAccounts()
        {
            return bankRepository.GetAccounts();
        }

        public Task<int> GetBalance(int accountID)
        {
            return bankRepository.GetBalanceByAccountID(accountID);
        }

        public Task<MiniStatement> GetMiniStatement(int accountID)
        {
            return couchbaseRepository.GetTransactionStatement(accountID);
        }

    }
}
