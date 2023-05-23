using Application;
using Infrastructure;
using Persistence;
using Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation()
    .AddPersistence();
builder.Host.UseSerilog(((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
}));
builder.Services.AddStackExchangeRedisCache(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connectionString;
});
builder
    .Services
    .AddControllers()
    .AddApplicationPart(Presentation.AssemblyReference.Assembly());
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();