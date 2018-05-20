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

namespace BankingService
{
    [ServiceBehavior(
  TransactionIsolationLevel =
    System.Transactions.IsolationLevel.Serializable,
  TransactionTimeout = "00:10:30",
  InstanceContextMode = InstanceContextMode.PerSession,
  TransactionAutoCompleteOnSessionClose = true)]
    public class AccountService : IAccountService
    {
        BankRepository bankRepository = new BankRepository();

        public AccountService()
        {
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Task<bool> Credit(int accountID, int amount)
        {
            return bankRepository.CreditAccount(accountID, amount);
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Task<bool> Debit(int accountID, int amount)
        {
            return bankRepository.DebitAccount(accountID,amount);
        }

        public Task<IEnumerable<Account>> GetAccounts()
        {
            return bankRepository.GetAccounts();
        }

        //[OperationBehavior]
        public Task<int> GetBalance(int accountID)
        {
            return bankRepository.GetBalanceByAccountID(accountID);
        }
    }
}
