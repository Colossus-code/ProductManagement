using Microsoft.Extensions.Caching.Memory;
using ProductManagement.Contracts.RepositoryContracts;
using ProductManagement.Contracts.ServiceContracts;
using ProductManagement.Implementations;
using ProductManagement.InfrastructureData;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache(memoryCacheOptions =>
{
    memoryCacheOptions.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
        SlidingExpiration = TimeSpan.FromMinutes(1.5)
    };
});
builder.Services.AddScoped<IProductManagementService, ProductManagementService>(); 
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<IProductTranslationRepository, ProductTranslationRepository>(); 
builder.Services.AddScoped<IProductCacheRepository, ProductCacheRepository>();


builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
