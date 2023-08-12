using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using MyExpenses.API.Data;
using MyExpenses.API.Mappings;
using MyExpenses.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BillsDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyExpensesConnectionString")));
builder.Services.AddDbContext<ExpensesAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyExpensesAuthConnectionString")));

builder.Services.AddScoped<IBillRepository, SQLBillRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("MyExpenses")
    .AddEntityFrameworkStores<ExpensesAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options=>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactFrontend",
        builder =>
        {
            builder.WithOrigins("https://danielbraaksma.github.io") // Replace with the actual URL of your React frontend
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials() // Allow sending cookies or credentials
                   .WithExposedHeaders("Authorization"); // Allow exposing Authorization header in response
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowReactFrontend");

app.Run();
