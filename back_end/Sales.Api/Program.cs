using Sales.Api.Extensions;
using Sales.Api.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

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

// Cau hinh OpenAPI native (thay the Swashbuckle)
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, context, ct) =>
    {
        doc.Info.Title = "Evo Sales API";
        doc.Info.Version = "v1";
        doc.Info.Description = "API quan ly ban hang: san pham, nhom hang, don vi tinh, don hang va kho hang.";
        return Task.CompletedTask;
    });
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
    // Endpoint tao OpenAPI document JSON: GET /openapi/v1.json
    app.MapOpenApi();

    // Scalar UI - giao dien API documentation hien dai
    // Truy cap tai: http://localhost:5000/scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.Title = "Evo Sales API";
        options.Theme = ScalarTheme.DeepSpace;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// Global Exception Handler Middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
