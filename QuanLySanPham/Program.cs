using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Infrastructure.Persistence.Commons;
using QuanLySanPham.Infrastructure.Persistence.Database;
using QuanLySanPham.Infrastructure.Persistence.Repositories;
using QuanLySanPham.Infrastructure.Security;
using QuanLySanPham.Presentations.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Đăng ký Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Nhập 'Bearer' + khoảng trắng + token của bạn.\n\nVí dụ: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...**"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
//Đăng ký JWT Filter
// đọc secret key từ appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey.ToString());
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // kiểm tra issuer
            ValidateAudience = true, // kiểm tra audience
            ValidateLifetime = true, // kiểm tra hạn token
            ValidateIssuerSigningKey = true, // kiểm tra chữ ký

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });
//Cấu hình Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomerOnly", policy => policy.RequireClaim("UserType","Customer"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("UserType","Employee"));
    options.AddPolicy("CustomerOrEmployee", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim("UserType", "Customer") ||
            context.User.HasClaim("UserType", "Employee")));
});

// Đăng ký Mediator
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
// đăng ký DI service
builder.Services.AddScoped<IDbContext, DbContext>();
/*builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDomainEventDispatcher, UnitOfWork>();
builder.Services.AddScoped<ITrackedEntities, UnitOfWork>();*/
builder.Services.AddScoped<UnitOfWork>(); // đăng ký gốc
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UnitOfWork>());
builder.Services.AddScoped<IDomainEventDispatcher>(sp => sp.GetRequiredService<UnitOfWork>());
builder.Services.AddScoped<ITrackedEntities>(sp => sp.GetRequiredService<UnitOfWork>());
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<ITourManagementRepository, TourManagementRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger UI sẽ hiển thị tại root
    });
}

//app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();