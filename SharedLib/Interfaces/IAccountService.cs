using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    [ServiceContract]    
    public interface IAccountService
    {
        [OperationContract]
        Task<IEnumerable<Account>> GetAccounts();

        [OperationContract, TransactionFlow(TransactionFlowOption.Mandatory)]
        Task<bool> Debit(int accountID, int amount);

        [OperationContract, TransactionFlow(TransactionFlowOption.Mandatory)]
        Task<bool> Credit(int accountID, int amount);

        [OperationContract]
        Task<int> GetBalance(int accountID);        
    }
}
