using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace PharmactTests.Services
{

    public class ServiceTests
    {
        [Fact]
        public void GetBill_ShouldReturnBill()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Insert test data into the in-memory database
                var sampleBill = new Bill { Order_ID = 1, Customer_SSN = "12345" };
                context.Bill.Add(sampleBill);
                context.SaveChanges();

                var service = new BillService(context);

                // Act
                var result = service.GetBill(1, "12345");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Order_ID);
            }
        }

        [Fact]
        public void GetAllBills_ShouldReturnListOfBills()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Insert test data into the in-memory database
                var sampleBill1 = new Bill { Order_ID = 1, Customer_SSN = "12345" };
                var sampleBill2 = new Bill { Order_ID = 2, Customer_SSN = "67890" };
                context.Bill.AddRange(sampleBill1, sampleBill2);
                context.SaveChanges();

                var service = new BillService(context);

                // Act
                var result = service.GetAllBills();

                // Assert
                Assert.NotEmpty(result);
                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public void GetAllCustomers_ReturnsAllCustomers()
        {
            // Arrange
            using var context = TestUtils.CreateAppDbContext(); // Create a test database context

            var service = new CustomerService(context);

            // Act
            var customers = service.GetAllCustomers().ToList();

            // Assert
            Assert.Equal(3, customers.Count); // Adjust the count as per your test data
                                              // You can add more specific assertions if needed
        }

        [Fact]
        public void GetCustomerBySSN_ReturnsCustomerWithMatchingSSN()
        {
            // Arrange
            using var context = TestUtils.CreateAppDbContext(); // Create a test database context

            var service = new CustomerService(context);

            // Act
            var customer = service.GetCustomerBySSN("123456789");

            // Assert
            Assert.NotNull(customer);
            Assert.Equal("John", customer.First_Name); // Adjust with the actual data in your test context
        }

        [Fact]
        public void AddCustomer_AddsCustomerToDatabase()
        {
            // Arrange
            using var context = TestUtils.CreateAppDbContext(); // Create a test database context

            var service = new CustomerService(context);

            var newCustomer = new Customer
            {
                SSN = "987654321",
                First_Name = "Alice",
                Last_Name = "Johnson",
                // Set other properties as needed
            };

            // Act
            service.AddCustomer(newCustomer);

            // Assert
            var addedCustomer = context.Customer.FirstOrDefault(c => c.SSN == newCustomer.SSN);
            Assert.NotNull(addedCustomer);
        }

        [Fact]
        public void UpdateCustomer_UpdatesCustomerInDatabase()
        {
            // Arrange
            using var context = TestUtils.CreateAppDbContext(); // Create a test database context

            var service = new CustomerService(context);

            var customerToUpdate = context.Customer.FirstOrDefault(c => c.SSN == "123456789");
            Assert.NotNull(customerToUpdate);
            var originalFirstName = customerToUpdate.First_Name;

            // Modify the customer's data
            customerToUpdate.First_Name = "UpdatedName";

            // Act
            service.UpdateCustomer(customerToUpdate);

            // Assert
            var updatedCustomer = context.Customer.FirstOrDefault(c => c.SSN == "123456789");
            Assert.NotNull(updatedCustomer);
            Assert.NotEqual(originalFirstName, updatedCustomer.First_Name);
            Assert.Equal("UpdatedName", updatedCustomer.First_Name);
        }

        [Fact]
        public void DeleteCustomer_RemovesCustomerFromDatabase()
        {
            // Arrange
            using var context = TestUtils.CreateAppDbContext(); // Create a test database context

            var service = new CustomerService(context);

            var customerToDelete = context.Customer.FirstOrDefault(c => c.SSN == "123456789");
            Assert.NotNull(customerToDelete);

            // Act
            service.DeleteCustomer(customerToDelete.SSN);

            // Assert
            var deletedCustomer = context.Customer.FirstOrDefault(c => c.SSN == "123456789");
            Assert.Null(deletedCustomer);
        }

        [Fact]
        public void GetAllMedicines_ReturnsAllMedicines()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllMedicines_Database")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var medicineService = new MedicineService(context);

                // Add test data to the in-memory database
                var testMedicine1 = new Medicine { Drug_Name = "Medicine1", Batch_Number = "Batch1" };
                var testMedicine2 = new Medicine { Drug_Name = "Medicine2", Batch_Number = "Batch2" };
                context.Medicine.AddRange(testMedicine1, testMedicine2);
                context.SaveChanges();

                // Act
                var medicines = medicineService.GetAllMedicines();

                // Assert
                Assert.Equal(2, medicines.Count());
            }
        }

        [Fact]
        public void GetMedicine_WithValidData_ReturnsMedicine()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetMedicine_Database")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var medicineService = new MedicineService(context);

                // Add a test medicine to the in-memory database
                var testMedicine = new Medicine { Drug_Name = "Medicine1", Batch_Number = "Batch1" };
                context.Medicine.Add(testMedicine);
                context.SaveChanges();

                // Act
                var medicine = medicineService.GetMedicine("Medicine1", "Batch1");

                // Assert
                Assert.NotNull(medicine);
                Assert.Equal("Medicine1", medicine.Drug_Name);
                Assert.Equal("Batch1", medicine.Batch_Number);
            }
        }

        [Fact]
        public void GetMedicine_WithInvalidData_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetMedicine_Database")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var medicineService = new MedicineService(context);

                // Act
                var medicine = medicineService.GetMedicine("NonExistentMedicine", "NonExistentBatch");

                // Assert
                Assert.Null(medicine);
            }
        }

        [Fact]
        public void AddMedicine_AddsMedicineToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AddMedicine_Database")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var medicineService = new MedicineService(context);

                // Act
                medicineService.AddMedicine(new Medicine { Drug_Name = "NewMedicine", Batch_Number = "NewBatch" });

                // Assert
                var addedMedicine = context.Medicine.FirstOrDefault(m => m.Drug_Name == "NewMedicine" && m.Batch_Number == "NewBatch");
                Assert.NotNull(addedMedicine);
            }
        }

        [Fact]
        public void UpdateMedicine_UpdatesMedicineInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateMedicine_Database")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var medicineService = new MedicineService(context);

                // Add a test medicine to the in-memory database
                var testMedicine = new Medicine { Drug_Name = "MedicineToUpdate", Batch_Number = "BatchToUpdate" };
                context.Medicine.Add(testMedicine);
                context.SaveChanges();

                // Act
                testMedicine.Price = 10.99m;
                medicineService.UpdateMedicine(testMedicine);

                // Assert
                var updatedMedicine = context.Medicine.FirstOrDefault(m => m.Drug_Name == "MedicineToUpdate" && m.Batch_Number == "BatchToUpdate");
                Assert.NotNull(updatedMedicine);
                Assert.Equal(10.99m, updatedMedicine.Price);
            }
        }

        [Fact]
        public void DeleteMedicine_DeletesMedicineFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteMedicine_Database")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var medicineService = new MedicineService(context);

                // Add a test medicine to the in-memory database
                var testMedicine = new Medicine { Drug_Name = "MedicineToDelete", Batch_Number = "BatchToDelete" };
                context.Medicine.Add(testMedicine);
                context.SaveChanges();

                // Act
                medicineService.DeleteMedicine("MedicineToDelete", "BatchToDelete");

                // Assert
                var deletedMedicine = context.Medicine.FirstOrDefault(m => m.Drug_Name == "MedicineToDelete" && m.Batch_Number == "BatchToDelete");
                Assert.Null(deletedMedicine);
            }
        }

        public class TestUtils
        {
            public static AppDbContext CreateAppDbContext()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use a unique name for each test
                    .Options;

                return new AppDbContext(options);
            }
        }
    }
}
