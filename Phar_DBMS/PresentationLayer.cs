using Microsoft.AspNetCore.Mvc;

[Route("api/bills")]
public class BillController : Controller
{
    private readonly BillService _billService;

    public BillController(BillService billService)
    {
        _billService = billService;
    }

    [HttpGet("{orderID}/{customerSSN}")]
    public IActionResult Get(int orderID, string customerSSN)
    {
        var bill = _billService.GetBill(orderID, customerSSN);
        if (bill == null)
        {
            return NotFound();
        }
        return Ok(bill);
    }

    [HttpGet]
    public IActionResult Get()
    {
        var bills = _billService.GetAllBills();
        return Ok(bills);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Bill bill)
    {
        _billService.CreateBill(bill);
        return CreatedAtAction("Get", new { orderID = bill.Order_ID, customerSSN = bill.Customer_SSN }, bill);
    }

    [HttpPut("{orderID}/{customerSSN}")]
    public IActionResult Put(int orderID, string customerSSN, [FromBody] Bill bill)
    {
        if (orderID != bill.Order_ID || customerSSN != bill.Customer_SSN)
        {
            return BadRequest();
        }
        _billService.UpdateBill(bill);
        return NoContent();
    }

    [HttpDelete("{orderID}/{customerSSN}")]
    public IActionResult Delete(int orderID, string customerSSN)
    {
        _billService.DeleteBill(orderID, customerSSN);
        return NoContent();
    }
}

[Route("api/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerRepository;

    public CustomerController(CustomerService customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public IActionResult GetAllCustomers()
    {
        var customers = _customerRepository.GetAllCustomers();
        return Ok(customers);
    }

    [HttpGet("{ssn}")]
    public IActionResult GetCustomerBySSN(string ssn)
    {
        var customer = _customerRepository.GetCustomerBySSN(ssn);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpPost]
    public IActionResult AddCustomer([FromBody] Customer customer)
    {
        _customerRepository.AddCustomer(customer);
        return CreatedAtAction("GetCustomerBySSN", new { ssn = customer.SSN }, customer);
    }

    [HttpPut("{ssn}")]
    public IActionResult UpdateCustomer(string ssn, [FromBody] Customer customer)
    {
        if (ssn != customer.SSN)
        {
            return BadRequest();
        }
        _customerRepository.UpdateCustomer(customer);
        return NoContent();
    }

    [HttpDelete("{ssn}")]
    public IActionResult DeleteCustomer(string ssn)
    {
        _customerRepository.DeleteCustomer(ssn);
        return NoContent();
    }
}

[Route("api/medicines")]
[ApiController]
public class MedicineController : ControllerBase
{
    private readonly MedicineService _medicineRepository;

    public MedicineController(MedicineService medicineRepository)
    {
        _medicineRepository = medicineRepository;
    }

    [HttpGet]
    public IActionResult GetAllMedicines()
    {
        var medicines = _medicineRepository.GetAllMedicines();
        return Ok(medicines);
    }

    [HttpGet("{drugName}/{batchNumber}")]
    public IActionResult GetMedicine(string drugName, string batchNumber)
    {
        var medicine = _medicineRepository.GetMedicine(drugName, batchNumber);
        if (medicine == null)
        {
            return NotFound();
        }
        return Ok(medicine);
    }

    [HttpPost]
    public IActionResult AddMedicine([FromBody] Medicine medicine)
    {
        _medicineRepository.AddMedicine(medicine);
        return CreatedAtAction("GetMedicine", new { drugName = medicine.Drug_Name, batchNumber = medicine.Batch_Number }, medicine);
    }

    [HttpPut("{drugName}/{batchNumber}")]
    public IActionResult UpdateMedicine(string drugName, string batchNumber, [FromBody] Medicine medicine)
    {
        if (drugName != medicine.Drug_Name || batchNumber != medicine.Batch_Number)
        {
            return BadRequest();
        }
        _medicineRepository.UpdateMedicine(medicine);
        return NoContent();
    }

    [HttpDelete("{drugName}/{batchNumber}")]
    public IActionResult DeleteMedicine(string drugName, string batchNumber)
    {
        _medicineRepository.DeleteMedicine(drugName, batchNumber);
        return NoContent();
    }
}

[Route("api/notifications")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationRepository;

    public NotificationController(NotificationService notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    [HttpGet]
    public IActionResult GetAllNotifications()
    {
        var notifications = _notificationRepository.GetAllNotifications();
        return Ok(notifications);
    }

    [HttpGet("{id}")]
    public IActionResult GetNotification(int id)
    {
        var notification = _notificationRepository.GetNotification(id);
        if (notification == null)
        {
            return NotFound();
        }
        return Ok(notification);
    }

    [HttpPost]
    public IActionResult AddNotification([FromBody] Notification notification)
    {
        _notificationRepository.AddNotification(notification);
        return CreatedAtAction("GetNotification", new { id = notification.ID }, notification);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateNotification(int id, [FromBody] Notification notification)
    {
        if (id != notification.ID)
        {
            return BadRequest();
        }
        _notificationRepository.UpdateNotification(notification);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteNotification(int id)
    {
        _notificationRepository.DeleteNotification(id);
        return NoContent();
    }
}

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<List<Order_Details>> GetAllOrders()
    {
        var orders = _orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order_Details> GetOrder(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public IActionResult CreateOrder(Order_Details order)
    {
        _orderService.CreateOrder(order);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Order_ID }, order);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOrder(int id, Order_Details order)
    {
        if (id != order.Order_ID)
            return BadRequest();

        _orderService.UpdateOrder(order);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        _orderService.DeleteOrder(id);
        return NoContent();
    }
}

[Route("api/ordereddrugs")]
[ApiController]
public class OrderedDrugController : ControllerBase
{
    private readonly OrderedDrugService _orderedDrugService;

    public OrderedDrugController(OrderedDrugService orderedDrugService)
    {
        _orderedDrugService = orderedDrugService;
    }

    [HttpGet("{orderId}")]
    public ActionResult<List<Ordered_Drugs>> GetOrderedDrugs(int orderId)
    {
        var orderedDrugs = _orderedDrugService.GetOrderedDrugsByOrderID(orderId);
        return Ok(orderedDrugs);
    }

    [HttpPost]
    public IActionResult AddOrderedDrug(Ordered_Drugs orderedDrug)
    {
        _orderedDrugService.AddOrderedDrug(orderedDrug);
        return CreatedAtAction(nameof(GetOrderedDrugs), new { orderId = orderedDrug.Order_ID }, orderedDrug);
    }

    [HttpPut]
    public IActionResult UpdateOrderedDrug(Ordered_Drugs orderedDrug)
    {
        _orderedDrugService.UpdateOrderedDrug(orderedDrug);
        return NoContent();
    }

    [HttpDelete("{orderId}/{drugName}/{batchNumber}")]
    public IActionResult DeleteOrderedDrug(int orderId, string drugName, string batchNumber)
    {
        _orderedDrugService.DeleteOrderedDrug(orderId, drugName, batchNumber);
        return NoContent();
    }
}

[Route("api/prescriptions")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly PrescriptionService _prescriptionService;

    public PrescriptionController(PrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet("{id}")]
    public ActionResult<List<Prescription>> GetPrescriptions(int id)
    {
        var prescriptions = _prescriptionService.GetPrescriptionsByID(id);
        return Ok(prescriptions);
    }

    [HttpPost]
    public IActionResult AddPrescription(Prescription prescription)
    {
        _prescriptionService.AddPrescription(prescription);
        return CreatedAtAction(nameof(GetPrescriptions), new { ssn = prescription.SSN }, prescription);
    }

    [HttpPut]
    public IActionResult UpdatePrescription(Prescription prescription)
    {
        _prescriptionService.UpdatePrescription(prescription);
        return NoContent();
    }

    [HttpDelete("{prescriptionId}")]
    public IActionResult DeletePrescription(int prescriptionId)
    {
        _prescriptionService.DeletePrescription(prescriptionId);
        return NoContent();
    }
}



