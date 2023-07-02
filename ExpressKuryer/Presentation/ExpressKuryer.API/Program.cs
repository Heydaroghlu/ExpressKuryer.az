using ExpressKuryer.Application;
using ExpressKuryer.Application.Enums;
using ExpressKuryer.Application.Middlewares;
using ExpressKuryer.Infrastructure;
using ExpressKuryer.Persistence;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureServices(StorageEnum.CloudinaryStorage);
builder.Services.AddApplicationServices();
builder.Services.AddCors(x => x.AddDefaultPolicy(policy => policy.WithOrigins("https://expresskuryer.az", "http://expresskuryer.az", "http://205.144.171.204", "http://localhost:5000", "https://localhost:5000", "http://127.0.0.1:5000", "https://127.0.0.1:5000").AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddAuthentication("ExpressMember")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
