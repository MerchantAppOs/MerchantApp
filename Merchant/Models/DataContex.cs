using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
    public class DataContex : DbContext
    {
        public DataContex() : base("DBConnection")
        {

        }
        public DbSet<Kataloq> Category { get; set; }
        public DbSet<Products> Product { get; set; }
        public DbSet<Costs> CostName { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Waherhouse>   Waherhouses { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Person>  People { get; set; }
        public DbSet<CashBox>  CashBoxes { get; set; }
        public DbSet<Purchase>   Purchases { get; set; }
        public DbSet<Sale>  Sales { get; set; }
        public DbSet<User> User { get; set; }
    }
    


    
}
