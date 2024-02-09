using Micro.Async.User.Interfaces;
using Micro.Async.User.Models;
using Micro.Async.User.Persistance;
using Micro.Async.User.Services;
using Micro.Async.User.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
JwtSettings _jwtSettings = new JwtSettings();
configuration.Bind(nameof(JwtSettings), _jwtSettings);
builder.Services.AddSingleton(_jwtSettings);

builder.Services.AddIdentity<User, AppRole>()
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageBroker, MessageBroker>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
builder.Services.AddControllers();

var app = builder.Build();
using (var serviceScope = app.Services.CreateScope())
{
    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();



    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var adminRole = new AppRole("Admin");
        await roleManager.CreateAsync(adminRole);
    }

    if (!await roleManager.RoleExistsAsync("User"))
    {
        var userRole = new AppRole("User");
        await roleManager.CreateAsync(userRole);
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
//var connection = factory.CreateConnection();
//using var channel = connection.CreateModel();
//channel.QueueDeclare(queue: "grades", durable: false, exclusive: false,
//    autoDelete: false, arguments: null);

//var consumer = new EventingBasicConsumer(channel);
//var result = "";
//consumer.Received += (sender, args) =>
//{
//    var body = args.Body.ToArray();
//    result = Encoding.UTF8.GetString(body);
//    Console.WriteLine($"{result}");
//};
//channel.BasicConsume(queue: "grades", autoAck: true, consumer: consumer);


app.Run();
