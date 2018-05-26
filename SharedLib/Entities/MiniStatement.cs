using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLib
{
    public class MiniStatement
    {        
        public int AccountID { get; set; }
        public IEnumerable<TransactionRecord> Transactions{ get; set; }
    }
}