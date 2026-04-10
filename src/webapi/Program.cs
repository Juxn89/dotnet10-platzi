using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using Scalar.AspNetCore;

using webapi.data;
using webapi.middlewares;
using webapi.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options => {
  var connectionString = builder.Configuration.GetConnectionString("SQL_DB");
  options.UseSqlServer(connectionString);
});

//builder.Services.AddDbContext<AppDbContext>(options => { 
//  options.UseInMemoryDatabase("WeatherForecastDb");
//});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["Jwt:Issuer"],
      ValidAudience = builder.Configuration["Jwt:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
      )
    };
  });

builder.Services.AddSwaggerGen(options => {
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Weather forecast API",
    Version = "v1",
    Description = "Handle weather forecast local list",
    Contact = new OpenApiContact
    {
      Name = "Juan Gómez",
      Email = "gb.jc@outlook.com"
    }
  });

  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  options.IncludeXmlComments(xmlPath);

  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    //Description = "Enter 'Bearer' [space] and then your valid token.",
    Description = "Enter your valid token."
  });

  
  options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
  {
    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
  });
});

var MyAllowOrigins = "MyAllowOrigins";
builder.Services.AddCors(options =>
{
  options.AddPolicy(MyAllowOrigins, config =>{
    config.AllowAnyHeader();
    config.AllowAnyOrigin();
    // config.WithOrigins("http://localhost:5173");
  });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
  });
}

app.UseCors(MyAllowOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequesLoggingMiddleware();

app.MapControllers();

app.Run();
