using Couchbase;
using Couchbase.Core;
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

    public class CouchbaseRepository
    {
        IBucket bucket;
        public CouchbaseRepository()
        {
            //ClusterHelper.Initialize("couchbaseClients/couchbase");
            bucket = ClusterHelper.GetBucket("Bank");
        }

        public virtual async Task<MiniStatement> GetTransactionStatement(int accountID)
        {
            //IBucket bucket = await ClusterHelper.GetBucketAsync("Bank");
            var miniStatement = await bucket.GetAsync<MiniStatement>(accountID.ToString());
            return miniStatement.Value?? new MiniStatement() {
                AccountID =accountID,
                Transactions = new List<TransactionRecord>() };
        }

        public virtual async Task<bool> CreditAccount(int accountID, TransactionRecord transactionRecord)
        {
            //IBucket bucket = await ClusterHelper.GetBucketAsync("Bank");
            var miniStatement = await GetTransactionStatement(accountID);
            var transactions = miniStatement.Transactions.ToList();
            transactions.Add(transactionRecord);
            miniStatement.Transactions = transactions;
            Document<MiniStatement> document = new Document<MiniStatement>
            {
                Id = miniStatement.AccountID.ToString(),
                Content = miniStatement
            };
            IDocumentResult<MiniStatement> upsert = await bucket.UpsertAsync(document);
            if (!upsert.Success)
            {
                return false;
                //throw upsert.Exception;
            }
            return true;
        }

        public virtual async Task<bool> DebitAccount(int accountID, TransactionRecord transactionRecord)
        {
            //IBucket bucket = await ClusterHelper.GetBucketAsync("Bank");
            var miniStatement = await GetTransactionStatement(accountID);
            var transactions = miniStatement.Transactions.ToList();
            transactions.Add(transactionRecord);
            miniStatement.Transactions = transactions;
            Document<MiniStatement> document = new Document<MiniStatement>
            {
                Id = miniStatement.AccountID.ToString(),
                Content = miniStatement
            };
            IDocumentResult<MiniStatement> upsert = await bucket.UpsertAsync(document);
            if (!upsert.Success)
            {
                return false;
                //throw upsert.Exception;
            }
            return true;
        }
    }
}
