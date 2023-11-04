using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<BillService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<MedicineService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderedDrugService>();
builder.Services.AddScoped<PrescriptionService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(@"Data Source=IN-100N0F3;Initial Catalog=Pharmacy;Integrated Security=True;trusted_connection=true;encrypt=false;");
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
