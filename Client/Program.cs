using BankingService;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine($"Debit account {accounts[0].AccountID} by 200");
                var debitSucess = await new ServiceClient<AccountService>().ExecuteAsync(x => x.Debit(accounts[0].AccountID, 200));
                Console.WriteLine($"Credit account {accounts[1].AccountID} by 200");
                var creditSucess = await new ServiceClient<AccountService>().ExecuteAsync(x => x.Credit(accounts[1].AccountID, 200));

                getAccounts = await new ServiceClient<AccountService>().ExecuteAsync(x => x.GetAccounts());
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
