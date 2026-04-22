using LayeredArchitecture_Task2_Catalog_Service.Business;
using LayeredArchitecture_Task2_Catalog_Service.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddBusinessServices();
builder.Services.AddRepositoryServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
