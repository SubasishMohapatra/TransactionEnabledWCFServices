﻿using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using SharedLib;

namespace Repository
{
    /// <summary>
    /// Der Context für EF
    /// </summary>
    public class BankContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DPEContext"/> class.
        /// </summary>
        public BankContext()
            : base("name=MyBank")
        {
#if DEBUG
            Database.Log = log => Debug.WriteLine(log);
#endif
            Database.SetInitializer<BankContext>(null);            
            Configuration.LazyLoadingEnabled = false;          
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public DbSet<Account> Account { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Account>().Property(p=>p.AccountID).HasColumnName("AccountID");
            modelBuilder.Entity<Account>().Property(p => p.CustomerName).HasColumnName("CustomerName");
            modelBuilder.Entity<Account>().Property(p => p.Balance).HasColumnName("Balance");
        }

    }
}