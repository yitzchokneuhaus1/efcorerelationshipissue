
using System.Linq;
using EFCoreDisconnectedEntitiesTest;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private DataContext sut;
        [TestInitialize]
        public void TestSetup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(connection, e => e.SuppressForeignKeyEnforcement())
                .Options;
            sut = new DataContext(options);

            sut.Database.EnsureDeleted();
            sut.Database.EnsureCreated();
            var employee = new Employee()
            {
                FirstName = "Test",
                LastName = "Person",
                EthnicityId = 1

            };

            sut.Employees.Add(employee);
            sut.SaveChanges();
        }


        [TestMethod]
        public void When_LoadingEntity_AsNoTracking_Updating_FK_SavesValueToDatabase()
        {
            var empFromDB = sut.Employees.Include(e => e.Ethnicity).AsNoTracking()
                 .FirstOrDefault(e => e.EmployeeId == 1);

            empFromDB.EthnicityId = 2;//Change the foreign key


            var keyProperty = sut.Model.FindEntityType(typeof(Employee)).FindPrimaryKey().Properties[0];
            var locallyTrackedEntity = sut.ChangeTracker.Entries<Employee>().FirstOrDefault(e => e.Property(keyProperty.Name).CurrentValue.ToString() == GetPropValue(empFromDB, keyProperty.Name).ToString());
            if (locallyTrackedEntity != null)
            {
                sut.Entry(locallyTrackedEntity.Entity).State = EntityState.Detached;
            }
             
            sut.Employees.Update(empFromDB);
            sut.SaveChanges();

            //load from database
            var empFromDB1 = sut.Employees.Include(e => e.Ethnicity)
                .FirstOrDefault(e => e.EmployeeId == 1);

            Assert.AreEqual(2, empFromDB1.EthnicityId);

        }


        [TestMethod]
        public void When_LoadingEntity_AsNoTracking_and_Missing_Includes_Updating_FK_SavesValueToDatabase()
        {
            var empFromDB = sut.Employees.AsNoTracking()
                 .FirstOrDefault(e => e.EmployeeId == 1);

            empFromDB.EthnicityId = 2;//Change the foreign key


            var keyProperty = sut.Model.FindEntityType(typeof(Employee)).FindPrimaryKey().Properties[0];
            var locallyTrackedEntity = sut.ChangeTracker.Entries<Employee>().FirstOrDefault(e => e.Property(keyProperty.Name).CurrentValue.ToString() == GetPropValue(empFromDB, keyProperty.Name).ToString());
            if (locallyTrackedEntity != null)
            {
                sut.Entry(locallyTrackedEntity.Entity).State = EntityState.Detached;
            }

            sut.Employees.Update(empFromDB);
            sut.SaveChanges();

            //load from database
            var empFromDB1 = sut.Employees.Include(e => e.Ethnicity)
                .FirstOrDefault(e => e.EmployeeId == 1);

            Assert.AreEqual(2, empFromDB1.EthnicityId);

        }




        [TestMethod]
        public void When_LoadingEntity_WITHOUT_AsNoTracking_Updating_FK_SavesValueToDatabase()
        {
            var empFromDB = sut.Employees.Include(e => e.Ethnicity)
                 .FirstOrDefault(e => e.EmployeeId == 1);

            empFromDB.EthnicityId = 2;//Change the foreign key


            var keyProperty = sut.Model.FindEntityType(typeof(Employee)).FindPrimaryKey().Properties[0];
            var locallyTrackedEntity = sut.ChangeTracker.Entries<Employee>().FirstOrDefault(e => e.Property(keyProperty.Name).CurrentValue.ToString() == GetPropValue(empFromDB, keyProperty.Name).ToString());
            if (locallyTrackedEntity != null)
            {
                sut.Entry(locallyTrackedEntity.Entity).State = EntityState.Detached;
            }

            sut.Employees.Update(empFromDB);
            sut.SaveChanges();

            //load from database
            var empFromDB1 = sut.Employees.Include(e => e.Ethnicity)
                .FirstOrDefault(e => e.EmployeeId == 1);

            Assert.AreEqual(2, empFromDB1.EthnicityId);

        }
         
    }
}
