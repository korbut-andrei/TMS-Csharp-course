using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Final_project.Entities.DbContexts;
using Final_project.Services;
using Final_project.Entities;
using Final_project.Models.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Security.Cryptography.X509Certificates;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Caching.Memory;
using Final_project.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console() 
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsoptions =>
    {
        var pfxPath = @"C:\c#\TMS-git\Final project\Final project\Certificates\selfsigned.pfx";
        var pfxPassword = "opensslpass";
        //var certPath = @"C:\c#\TMS-git\Final project\Final project\Certificates\selfsigned.crt";
        //var keyPath = @"C:\c#\TMS-git\Final project\Final project\Certificates\selfsigned.key";

        httpsoptions.ServerCertificate = new X509Certificate2(pfxPath, pfxPassword);
    });
});

ConfigurationManager configuration = builder.Configuration;

configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

var connectionString = configuration["ConnectionStrings:ConnStr"] ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
var jwtSecret = configuration["JWT:Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");

builder.Services.AddDbContext<CareerContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<UserEntity, ApplicationRole>()
    .AddEntityFrameworkStores<CareerContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var _logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

            Console.WriteLine("Claims:");
            foreach (var claim in context.Principal.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            _logger.LogDebug("Claims:");
            foreach (var claim in context.Principal.Claims)
            {
                _logger.LogDebug($"{claim.Type}: {claim.Value}");
            }

            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<UserEntity>>();
            var userName = context.Principal.FindFirstValue(ClaimTypes.Name);

            Console.WriteLine($"userName: {userName}");
            _logger.LogDebug($"userName: {userName}");


            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                context.Fail("Unauthorized");
                return;
            }

            var securityStamp = context.Principal.FindFirstValue("SecutiyStamp");

            if (securityStamp != user.SecurityStamp)
            {
                context.Fail("Unauthorized");
            }
        },
        OnChallenge = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                message = "JWT token has not passed the security check."
            }
                );
            return context.Response.WriteAsync(result);
        }

    };
});

builder.Services.AddMemoryCache();
builder.Services.AddScoped<HashHelper>();
builder.Services.AddScoped<CareerService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<DbRecordsCheckService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<RequestTimingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();