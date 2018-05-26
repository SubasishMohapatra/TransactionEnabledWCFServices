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

        [OperationContract, TransactionFlow(TransactionFlowOption.Allowed)]
        Task<bool> Debit(int accountID, int amount);

        [OperationContract, TransactionFlow(TransactionFlowOption.Allowed)]
        Task<bool> Credit(int accountID, int amount);

        [OperationContract]
        Task<int> GetBalance(int accountID);

        [OperationContract]
        Task<MiniStatement> GetMiniStatement(int accountID);
    }
}
