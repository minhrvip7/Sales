using Sales.Api.Extensions;
using Sales.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add Database
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add Dependency Injection (Repositories, Services, Mapper)
builder.Services.AddDependencyInjection();

// Configure routing to use lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add Controllers with NewtonsoftJson formatting
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Evo Sales API",
        Version = "v1",
        Description = "API quản lý bán hàng: sản phẩm, nhóm hàng, đơn vị tính, đơn hàng và khách hàng."
    });

    // Bật hỗ trợ [SwaggerOperation], [SwaggerResponse] attribute
    c.EnableAnnotations();

    // Load XML comment từ Sales.Api
    var apiXml = Path.Combine(AppContext.BaseDirectory, "Sales.Api.xml");
    if (File.Exists(apiXml)) c.IncludeXmlComments(apiXml);

    // Load XML comment từ Sales.Application (DTOs)
    var appXml = Path.Combine(AppContext.BaseDirectory, "Sales.Application.xml");
    if (File.Exists(appXml)) c.IncludeXmlComments(appXml);

    // Load XML comment từ Sales.Domain (Entities, Enums)
    var domainXml = Path.Combine(AppContext.BaseDirectory, "Sales.Domain.xml");
    if (File.Exists(domainXml)) c.IncludeXmlComments(domainXml);

    // Hiển thị enum dưới dạng tên string + mô tả thay vì số nguyên
    c.UseInlineDefinitionsForEnums();
});

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evo Sales API v1");
    });
}

// Global Exception Handler Middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

