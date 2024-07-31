using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using TestApi;
using TestApi.Data;
using TestApi.Services.ProductService;
using TestApi.Services.ValidatorService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme{
        Description = "Standard Authorization header using the Bearer scheme, e.g. \"Bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Host.UseSerilog((hostContext, services, configuration) => {
    // configuration.WriteTo.Console();
    configuration.ReadFrom.Configuration(hostContext.Configuration);
    configuration.ReadFrom.Services(services);
    configuration.Enrich.FromLogContext();
});

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IValidatorService, ValidatorService>();
builder.Services.AddScoped<IProductService, ProductService>();


var mapperConfig = new MapperConfiguration(mc =>
{
    mc.Internal().MethodMappingEnabled = false;
    mc.AddProfile(new AutoMapperProfile());
});
builder.Services.AddAutoMapper(cfg => cfg.Internal().MethodMappingEnabled = false, typeof(AutoMapperProfile).Assembly);

var connectionString = $"Server={Env.DbHost()}; Database={Env.DbName()}; User Id={Env.DbUser()};Password={Env.DbPassword()}; Encrypt=True; TrustServerCertificate=True;";
//var connectionString = $"Server={Env.DbHost()}; Database={Env.DbName()}; User Id={Env.DbUser()};Password={Env.DbPassword()}; Encrypt=True; Initial Catalog=Clinic; Integrated Security=true;";
//var connectionString = $"Server=localhost; Database=TestDB; User Id=sa;Password=Admin123; Encrypt=True; TrustServerCertificate=True;";
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.UseCompatibilityLevel(110);
        sqlServerOptions.CommandTimeout(180);
    })
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Env.AppToken())),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
    builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddControllers();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.EnableAnnotations();
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "livingos_erp_backend", Version = "v1" });
//     c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//     {
//         Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
//         In = ParameterLocation.Header,
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey
//     });
//     c.OperationFilter<SecurityRequirementsOperationFilter>();
// });



var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();


// app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseSerilogRequestLogging();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
