using DatingAppApi.Data;
using DatingAppApi.Extensions;
using DatingAppApi.Interfaces;
using DatingAppApi.Middleware;
using DatingAppApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

//This AddApplicationServices is an extension method for below codes of AddDbContext,AddCors
//and AddScoped
builder.Services.AddApplicationServices(builder.Configuration);

//builder.Services.AddDbContext<DataContext>(item =>
//{
//    item.UseSqlServer(builder.Configuration.GetConnectionString("defaultconnection"));
//});
//builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
//{
//    build.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
//} ));
//builder.Services.AddScoped<ITokenService, TokenService>();

//The AddIdentityServices method is an extension method of AddAuthentication
builder.Services.AddIdentityServices(builder.Configuration);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding
//                .UTF8.GetBytes(builder.Configuration["TokenKey"])),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration!");
}

app.Run();
