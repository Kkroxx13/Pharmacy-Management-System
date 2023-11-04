using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class Bill
{
    [Key]
    public int Order_ID { get; set; }
    public string Customer_SSN { get; set; }
    public decimal? Total_Amount { get; set; }
    public decimal? Customer_Payment { get; set; }
}

public class Customer
{
    [Key]
    public string SSN { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string Phone { get; set; }
    public char? Gender { get; set; }
    public string Address { get; set; }
    public DateTime? Date_of_Birth { get; set; }
}

public class Medicine
{
    [Key]
    public string Drug_Name { get; set; }

    [Required]
    public string Batch_Number { get; set; }
    public string Medicine_Type { get; set; }
    public string Manufacturer { get; set; }
    public int Quantity { get; set; }
    public DateTime? Expiry_Date { get; set; }
    public decimal? Price { get; set; }
}

public class Notification
{
    public int ID { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }
}

public class Order_Details
{
    [Key]
    public int Order_ID { get; set; }
    public int Prescription_ID { get; set; }
    public int Employee_ID { get; set; }
    public DateTime Order_Date { get; set; }
}

public class Ordered_Drugs
{
    [Key]
    public int Order_ID { get; set; }
    public string Drug_Name { get; set; }
    public string Batch_Number { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
}

public class Prescription
{
    [Key]
    public int Prescription_ID { get; set; }
    public string SSN { get; set; }
    public int Doctor_ID { get; set; }
    public DateTime? Prescription_Date { get; set; }
}


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Bill> Bill { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Medicine> Medicine { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Order_Details> Order_Details { get; set; }
    public DbSet<Ordered_Drugs> Ordered_Drugs { get; set; }
    public DbSet<Prescription> Prescription { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(@"Data Source=IN-100N0F3;Initial Catalog=Pharmacy;Integrated Security=True;trusted_connection=true;encrypt=false;");
    }
}
