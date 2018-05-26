using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLib
{
    public class TransactionRecord
    {
        public DateTime TransactionDate { get; set; }
        public string OperationType { get; set; }
        public bool IsSuccess{ get; set; }
        public int TransactionAmount { get; set; }
        public int Balance { get; set; }
    }
}