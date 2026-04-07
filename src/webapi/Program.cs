using System.Reflection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using webapi.middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

  options.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
  {
    Type = SecuritySchemeType.Http,
    Scheme = "basic",
    In = ParameterLocation.Header,
    Description = "Basic Authentication header. Use the format username/password"
  });

  options.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
    { 
      new OpenApiSecuritySchemeReference("basic", document),
      new List<string>()
    }
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

app.UseBasicAuthMiddleware();

app.UseAuthorization();

app.UseRequesLoggingMiddleware();

app.MapControllers();

app.Run();
