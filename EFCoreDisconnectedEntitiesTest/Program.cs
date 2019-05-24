using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Castle.Core.Logging;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
namespace EFCoreDisconnectedEntitiesTest
{
    class Program
    {


       
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            // In - memory database only exists while the connection is open
         

                //load from database
             
             

            Console.ReadLine();
        }
    }


    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Ethnicity> Ethnicities { get; set; }
       public static readonly Microsoft.Extensions.Logging.ILoggerFactory consoleLoggerFactory
         = new LoggerFactory().AddConsole();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(consoleLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ethnicity>().HasData(new[]
            {
                new Ethnicity(){Description = "Caucasian", EthnicityId = 1},
                new Ethnicity() {EthnicityId = 2, Description = "Hispanic"}
            });
        }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("EthnicityId")]
        public virtual Ethnicity Ethnicity { get; set; }
        public int EthnicityId { get; set; }
    }

    public class Ethnicity
    {
        public int EthnicityId { get; set; }
        public string Description { get; set; }
    }
}
