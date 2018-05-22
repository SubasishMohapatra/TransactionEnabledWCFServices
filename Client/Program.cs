using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var getAccounts = await new ServiceClient<IAccountService>().ExecuteAsync(x => x.GetAccounts());
                var accounts = getAccounts.ToList();
                accounts.ForEach(x =>
                {
                    Console.WriteLine($"AccountID:{x.AccountID}, CustomerName:{x.CustomerName}, Balance:{x.Balance}");
                });
                var txOptions = new TransactionOptions();
                txOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                //TransactionScopeAsyncFlowOption.Enabled,
                using (var ts = new TransactionScope(TransactionScopeOption.Required, txOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        Console.WriteLine($"Debit customer {accounts[0].CustomerName } with account# {accounts[0].AccountID} by 200");
                        var debitSucess = await new ServiceClient<IAccountService>().ExecuteAsync(x => x.Debit(accounts[0].AccountID, 200));
                        Console.WriteLine($"Credit customer {accounts[0].CustomerName } with account# {accounts[1].AccountID} by 200");
                        var creditSucess = await new ServiceClient<IAccountService>().ExecuteAsync(x => x.Credit(accounts[1].AccountID, 200));
                        if (debitSucess && creditSucess)
                            ts.Complete();
                        else
                            throw new Exception("Failed transaction");
                    }
                    catch (Exception ex)
                    {
                        ts.Dispose();
                        Console.WriteLine("Transaction unsuccessful. Rollback initiated");
                    }
                }
                getAccounts = await new ServiceClient<IAccountService>().ExecuteAsync(x => x.GetAccounts());
                accounts = getAccounts.ToList();
                accounts.ForEach(x =>
                {
                    Console.WriteLine($"AccountID:{x.AccountID}, CustomerName:{x.CustomerName}, Balance:{x.Balance}");
                });
            }).Wait();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
