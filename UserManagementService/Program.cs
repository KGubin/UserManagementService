using Microsoft.EntityFrameworkCore;
using UserManagementService.Repositories;
using UserManagementService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var dataProvider = builder.Configuration.GetValue<string>("DataProvider");
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("InMemoryDatabase");

if (useInMemoryDatabase)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("InMemoryDb"));
}
else
{
    if (dataProvider == "MsSql")
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection")));
    }
    else if (dataProvider == "PostgreSQL")
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
    }
}

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();