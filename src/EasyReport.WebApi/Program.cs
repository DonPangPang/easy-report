using EasyReport.WebApi.Cache;
using EasyReport.WebApi.Configurations;
using EasyReport.WebApi.Data;
using EasyReport.WebApi.Extensions;
using EasyReport.WebApi.Filters;
using EasyReport.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Soda.AutoMapper;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.Configure<AppSettings>(configuration);

builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(Assembly.Load("EasyReport.Shared"));
builder.Services.AddDbContext<EasyReportDbContext>();

builder.Services.AddAuthentication(setup =>
{
    setup.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    setup.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    setup.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = Consts.Jwt.Issuer,
        ValidateAudience = true,
        ValidAudience = Consts.Jwt.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Consts.Jwt.SecurityKey)), //SecurityKey
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30),
        RequireExpirationTime = true,
    };

    options.SaveToken = true;

    options.Events = new JwtBearerEvents()
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Consts.DefaultPermission, policy =>
    {
        policy.Requirements.Add(new PermissionRequirement());
    });
});
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddScoped<ICurrentUserSession, CurrentUserSession>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddStackExchangeRedisCache(setup =>
{
    setup.Configuration = configuration.GetSection("Redis").Value;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(Consts.DefaultCorsPolicyName, policy =>
    {
        policy.AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<GlobalExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Document", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Input Token: Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


var app = builder.Build();

app.InitSodaMapper();
app.InitDatabase(true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{

    try
    {
        await next();
    }
    catch (UnauthorizedAccessException)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
    }
});

app.UseCors(Consts.DefaultCorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
