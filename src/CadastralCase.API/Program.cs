using Amazon.DynamoDBv2;
using CadastralCase.Application.Services;
using CadastralCase.Domain.Interfaces;
using CadastralCase.Infrastructure.Data;
using CadastralCase.Infrastructure.ExternalServices;
using CadastralCase.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonDynamoDB>();

builder.Services.AddHttpClient<IViaCepService, ViaCepService>();

builder.Services.AddScoped<INaturalPersonRepository, NaturalPersonRepository>();
builder.Services.AddScoped<ILegalPersonRepository, LegalPersonRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepositoryDynamoDB>();

builder.Services.AddScoped<NaturalPersonService>();
builder.Services.AddScoped<LegalPersonService>();
builder.Services.AddScoped<AddressService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cadastral Case API",
        Version = "v1",
        Description = "API for managing registrations of natural and legal persons with hexagonal architecture",
        Contact = new OpenApiContact
        {
            Name = "Pietro Brum",
            Email = "pietrobcj@gmail.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastral Case API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    version = "1.0.0"
}))
.WithName("HealthCheck")
.WithTags("Health");

app.Run();
