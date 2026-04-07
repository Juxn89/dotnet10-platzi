using System.Reflection;

using Microsoft.OpenApi;

using Scalar.AspNetCore;

using webapi.middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
  app.UseSwaggerUI();
}

app.UseCors(MyAllowOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

// Custom middleware can be added here
app.UseRequesLoggingMiddleware();

app.MapControllers();

app.Run();
