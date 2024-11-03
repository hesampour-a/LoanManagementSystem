using LoanManagementSystem.Persistence.Ef;
using LoanManagementSystem.Persistence.Ef.Admins;
using LoanManagementSystem.Persistence.Ef.Customers;
using LoanManagementSystem.Persistence.Ef.LoanFormats;
using LoanManagementSystem.Persistence.Ef.UnitOfWorks;
using LoanManagementSystem.Services.Admins;
using LoanManagementSystem.Services.Admins.Contracts;
using LoanManagementSystem.Services.Customers;
using LoanManagementSystem.Services.Customers.Contracts;
using LoanManagementSystem.Services.LoanFormats;
using LoanManagementSystem.Services.LoanFormats.Contracts;
using LoanManagementSystem.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EfDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<AdminService, AdminAppService>();
builder.Services.AddScoped<AdminQuery, EFAdminQuery>();
builder.Services.AddScoped<AdminRepository, EFAdminRepository>();
builder.Services.AddScoped<CustomerService, CustomerAppService>();
builder.Services.AddScoped<CustomerQuery, EFCustomerQuery>();
builder.Services.AddScoped<CustomerRepository, EFCustomerRepository>();
builder.Services.AddScoped<LoanFormatService, LoanFormatAppService>();
builder.Services.AddScoped<LoanFormatRepository, EFLoanFormatRepository>();
builder.Services.AddScoped<LoanFormatQuery, EFLoanFormatQuery>();

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