using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace PharmactTests.Controllers
{
    public class BillControllerTests
    {
        private readonly BillController _billController;

        public BillControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var billService = new BillService(context);
                _billController = new BillController(billService);
            }

        }

        [Fact]
        public void Get_WithValidOrderAndCustomer_ReturnsOk()
        {
            // Arrange: Provide valid orderID and customerSSN for an existing bill
            int orderID = 1;
            string customerSSN = "123456789";

            // Act: Call the Get action with valid parameters
            IActionResult result = _billController.Get(orderID, customerSSN);

            // Assert: Verify that the result is an OkObjectResult
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Get_WithInvalidOrderAndCustomer_ReturnsNotFound()
        {
            // Arrange: Provide invalid orderID and customerSSN
            int orderID = 99; // An order that doesn't exist
            string customerSSN = "987654321"; // A non-existent customer

            // Act: Call the Get action with invalid parameters
            IActionResult result = _billController.Get(orderID, customerSSN);

            // Assert: Verify that the result is NotFound
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAllBills_ReturnsOk()
        {
            // Act: Call the Get action without parameters
            IActionResult result = _billController.Get();

            // Assert: Verify that the result is an OkObjectResult
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Post_WithValidBill_ReturnsCreatedAtAction()
        {
            // Arrange: Create a new Bill object with valid data
            var newBill = new Bill
            {
                Order_ID = 2,
                Customer_SSN = "987654321", // Use a non-existent SSN to avoid conflicts
                                            // Set other properties accordingly
            };

            // Act: Call the Post action with the new Bill object
            IActionResult result = _billController.Post(newBill);

            // Assert: Verify that the result is CreatedAtAction
            Assert.IsType<CreatedAtActionResult>(result);

            // Additional Assertions if needed:
            var createdResult = (CreatedAtActionResult)result;
            var createdBill = (Bill)createdResult.Value;
            Assert.Equal(newBill.Order_ID, createdBill.Order_ID);
            Assert.Equal(newBill.Customer_SSN, createdBill.Customer_SSN);
            // Add more assertions for other properties as needed
        }

        [Fact]
        public void Put_WithValidData_ReturnsNoContent()
        {
            // Arrange: Create a Bill object with valid data and existing orderID and customerSSN
            var existingBill = new Bill
            {
                Order_ID = 1, // An existing orderID
                Customer_SSN = "123456789", // An existing SSN
                                            // Set other properties accordingly
            };

            // Act: Call the Put action with the existing Bill object
            IActionResult result = _billController.Put(existingBill.Order_ID, existingBill.Customer_SSN, existingBill);

            // Assert: Verify that the result is NoContent
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Put_WithMismatchedData_ReturnsBadRequest()
        {
            // Arrange: Create a Bill object with valid data and different orderID and customerSSN
            var existingBill = new Bill
            {
                Order_ID = 1, // An existing orderID
                Customer_SSN = "123456789", // An existing SSN
                                            // Set other properties accordingly
            };

            // Act: Call the Put action with different orderID and customerSSN
            IActionResult result = _billController.Put(2, "987654321", existingBill);

            // Assert: Verify that the result is BadRequest
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_WithValidOrderAndCustomer_ReturnsNoContent()
        {
            // Arrange: Provide valid orderID and customerSSN for an existing bill
            int orderID = 1;
            string customerSSN = "123456789";

            // Act: Call the Delete action with valid parameters
            IActionResult result = _billController.Delete(orderID, customerSSN);

            // Assert: Verify that the result is NoContent
            Assert.IsType<NoContentResult>(result);
        }
    }

    public class CustomerControllerTests
    {
        private readonly CustomerController _customerController;

        public CustomerControllerTests()
        {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var customerService = new CustomerService(context);
                _customerController = new CustomerController(customerService);
            }
        }

        [Fact]
        public void GetAllCustomers_ReturnsOk()
        {
            // Act: Call the GetAllCustomers action
            IActionResult result = _customerController.GetAllCustomers();

            // Assert: Verify that the result is an OkObjectResult
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomerBySSN_WithValidSSN_ReturnsOk()
        {
            // Arrange: Provide a valid SSN for an existing customer
            string validSSN = "123456789"; // An existing SSN in your test data

            // Act: Call the GetCustomerBySSN action with the valid SSN
            IActionResult result = _customerController.GetCustomerBySSN(validSSN);

            // Assert: Verify that the result is an OkObjectResult
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCustomerBySSN_WithInvalidSSN_ReturnsNotFound()
        {
            // Arrange: Provide an invalid SSN
            string invalidSSN = "987654321"; // A non-existent SSN

            // Act: Call the GetCustomerBySSN action with the invalid SSN
            IActionResult result = _customerController.GetCustomerBySSN(invalidSSN);

            // Assert: Verify that the result is NotFound
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddCustomer_WithValidCustomer_ReturnsCreatedAtAction()
        {
            // Arrange: Create a new Customer object with valid data
            var newCustomer = new Customer
            {
                SSN = "987654321", // Use a non-existent SSN to avoid conflicts
                                   // Set other properties accordingly
            };

            // Act: Call the AddCustomer action with the new Customer object
            IActionResult result = _customerController.AddCustomer(newCustomer);

            // Assert: Verify that the result is CreatedAtAction
            Assert.IsType<CreatedAtActionResult>(result);

            // Additional Assertions if needed:
            var createdResult = (CreatedAtActionResult)result;
            var createdCustomer = (Customer)createdResult.Value;
            Assert.Equal(newCustomer.SSN, createdCustomer.SSN);
            // Add more assertions for other properties as needed
        }

        [Fact]
        public void UpdateCustomer_WithValidData_ReturnsNoContent()
        {
            // Arrange: Create a Customer object with valid data and an existing SSN
            var existingCustomer = new Customer
            {
                SSN = "123456789", // An existing SSN
                                   // Set other properties accordingly
            };

            // Act: Call the UpdateCustomer action with the existing Customer object
            IActionResult result = _customerController.UpdateCustomer(existingCustomer.SSN, existingCustomer);

            // Assert: Verify that the result is NoContent
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateCustomer_WithMismatchedData_ReturnsBadRequest()
        {
            // Arrange: Create a Customer object with valid data and a different SSN
            var existingCustomer = new Customer
            {
                SSN = "123456789", // An existing SSN
                                   // Set other properties accordingly
            };

            // Act: Call the UpdateCustomer action with a different SSN
            IActionResult result = _customerController.UpdateCustomer("987654321", existingCustomer);

            // Assert: Verify that the result is BadRequest
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteCustomer_WithValidSSN_ReturnsNoContent()
        {
            // Arrange: Provide a valid SSN for an existing customer
            string validSSN = "123456789"; // An existing SSN in your test data

            // Act: Call the DeleteCustomer action with the valid SSN
            IActionResult result = _customerController.DeleteCustomer(validSSN);

            // Assert: Verify that the result is NoContent
            Assert.IsType<NoContentResult>(result);
        }
    }
}
