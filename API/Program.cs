using API.Middleware;
using Core.Interface;
using Infrastructure.Data;
using Infrastructure.Implemention;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json",optional: false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,true);
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddCors();
builder.Services.AddSingleton<IConnectionMultiplexer>(config => {
    var connString = builder.Configuration.GetConnectionString("Redis") 
            ?? throw new Exception("Cannot get redis connection string");
    var configuration =  ConfigurationOptions.Parse(connString,true);

    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICartService,CartService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Configure the HTTP request pipeline
app.UseMiddleware<ExeceptionMiddleware>();

app.UseMiddleware<RequestTimingMiddleware>();
app.UseCors(x=>x.AllowAnyHeader ().AllowAnyMethod()
    .WithOrigins("http://localhost:4200","https://localhost:4200")
);
app.MapControllers();
try
{
    var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}
app.Run();
