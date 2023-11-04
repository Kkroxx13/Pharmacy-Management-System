using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class BillService
{
    private readonly AppDbContext _context;

    public BillService(AppDbContext context)
    {
        _context = context;
    }

    public Bill GetBill(int orderID, string customerSSN)
    {
        return _context.Bill.FirstOrDefault(b => b.Order_ID == orderID && b.Customer_SSN == customerSSN);
    }

    public List<Bill> GetAllBills()
    {
        return _context.Bill.ToList();
    }

    public void CreateBill(Bill bill)
    {
        _context.Bill.Add(bill);
        _context.SaveChanges();
    }

    public void UpdateBill(Bill bill)
    {
        _context.Bill.Update(bill);
        _context.SaveChanges();
    }

    public void DeleteBill(int orderID, string customerSSN)
    {
        var bill = _context.Bill.FirstOrDefault(b => b.Order_ID == orderID && b.Customer_SSN == customerSSN);
        if (bill != null)
        {
            _context.Bill.Remove(bill);
            _context.SaveChanges();
        }
    }
}

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _context.Customer.ToList();
    }

    public Customer GetCustomerBySSN(string ssn)
    {
        return _context.Customer.FirstOrDefault(c => c.SSN == ssn);
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customer.Add(customer);
        _context.SaveChanges();
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customer.Update(customer);
        _context.SaveChanges();
    }

    public void DeleteCustomer(string ssn)
    {
        var customer = _context.Customer.FirstOrDefault(c => c.SSN == ssn);
        if (customer != null)
        {
            _context.Customer.Remove(customer);
            _context.SaveChanges();
        }
    }
}

public class MedicineService
{
    private readonly AppDbContext _context;

    public MedicineService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Medicine> GetAllMedicines()
    {
        return _context.Medicine.ToList();
    }

    public Medicine GetMedicine(string drugName, string batchNumber)
    {
        return _context.Medicine.FirstOrDefault(m => m.Drug_Name == drugName && m.Batch_Number == batchNumber);
    }

    public void AddMedicine(Medicine medicine)
    {
        _context.Medicine.Add(medicine);
        _context.SaveChanges();
    }

    public void UpdateMedicine(Medicine medicine)
    {
        _context.Medicine.Update(medicine);
        _context.SaveChanges();
    }

    public void DeleteMedicine(string drugName, string batchNumber)
    {
        var medicine = _context.Medicine.FirstOrDefault(m => m.Drug_Name == drugName && m.Batch_Number == batchNumber);
        if (medicine != null)
        {
            _context.Medicine.Remove(medicine);
            _context.SaveChanges();
        }
    }
}

public class NotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Notification> GetAllNotifications()
    {
        // Use a stored procedure to get all notifications
        return _context.Notification.FromSqlRaw("EXEC GetNotifications").ToList();
    }

    public Notification GetNotification(int id)
    {
        // Use a stored procedure to get a notification by ID
        return _context.Notification.FromSqlRaw("EXEC GetNotification {0}", id).AsEnumerable().FirstOrDefault();
    }

    public void AddNotification(Notification notification)
    {
        // Use a stored procedure to create (insert) a notification
        _context.Database.ExecuteSqlRaw("EXEC CreateNotification {0}, {1}", notification.Message, notification.Type);
    }

    public void UpdateNotification(Notification notification)
    {
        // Use a stored procedure to update a notification
        _context.Database.ExecuteSqlRaw("EXEC UpdateNotification {0}, {1}, {2}", notification.ID, notification.Message, notification.Type);
    }

    public void DeleteNotification(int id)
    {
        // Use a stored procedure to delete a notification
        _context.Database.ExecuteSqlRaw("EXEC DeleteNotification {0}", id);
    }
}

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public List<Order_Details> GetAllOrders()
    {
        return _context.Order_Details.ToList();
    }

    public Order_Details GetOrderById(int orderId)
    {
        return _context.Order_Details.FirstOrDefault(o => o.Order_ID == orderId);
    }

    public void CreateOrder(Order_Details order)
    {
        _context.Order_Details.Add(order);
        _context.SaveChanges();
    }

    public void UpdateOrder(Order_Details order)
    {
        _context.Order_Details.Update(order);
        _context.SaveChanges();
    }

    public void DeleteOrder(int orderId)
    {
        var order = _context.Order_Details.FirstOrDefault(o => o.Order_ID == orderId);
        if (order != null)
        {
            _context.Order_Details.Remove(order);
            _context.SaveChanges();
        }
    }
}
public class OrderedDrugService
{
    private readonly AppDbContext _context;

    public OrderedDrugService(AppDbContext context)
    {
        _context = context;
    }

    public List<Ordered_Drugs> GetOrderedDrugsByOrderID(int orderId)
    {
        return _context.Ordered_Drugs.Where(o => o.Order_ID == orderId).ToList();
    }

    public void AddOrderedDrug(Ordered_Drugs orderedDrug)
    {
        _context.Ordered_Drugs.Add(orderedDrug);
        _context.SaveChanges();
    }

    public void UpdateOrderedDrug(Ordered_Drugs orderedDrug)
    {
        _context.Ordered_Drugs.Update(orderedDrug);
        _context.SaveChanges();
    }

    public void DeleteOrderedDrug(int orderId, string drugName, string batchNumber)
    {
        var orderedDrug = _context.Ordered_Drugs
            .FirstOrDefault(o => o.Order_ID == orderId && o.Drug_Name == drugName && o.Batch_Number == batchNumber);
        if (orderedDrug != null)
        {
            _context.Ordered_Drugs.Remove(orderedDrug);
            _context.SaveChanges();
        }
    }
}

public class PrescriptionService
{
    private readonly AppDbContext _context;

    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }

    public List<Prescription> GetPrescriptionsByID(int id)
    {
        return _context.Prescription.Where(p => p.Prescription_ID == id).ToList();
    }

    public void AddPrescription(Prescription prescription)
    {
        _context.Prescription.Add(prescription);
        _context.SaveChanges();
    }

    public void UpdatePrescription(Prescription prescription)
    {
        _context.Prescription.Update(prescription);
        _context.SaveChanges();
    }

    public void DeletePrescription(int prescriptionId)
    {
        var prescription = _context.Prescription.Find(prescriptionId);
        if (prescription != null)
        {
            _context.Prescription.Remove(prescription);
            _context.SaveChanges();
        }
    }
}






