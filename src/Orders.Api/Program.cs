using Orders.Api.Clients;
using Orders.Api.Http;
using Orders.Api.Middleware;
using Orders.Api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// var catalogBaseUrl = builder.Configuration["CatalogApi:BaseUrl"]
//     ?? throw new InvalidOperationException("Missing configuration: CatalogApi:BaseUrl");

// builder.Services.AddHttpClient<CatalogApiClient>(http =>
// {
//     http.BaseAddress = new Uri(catalogBaseUrl);
// });

builder.Services.AddOptions<CatalogApiOptions>()
    .Bind(builder.Configuration.GetSection(CatalogApiOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl), "CatalogApi:BaseUrl is required")
    .ValidateOnStart();

builder.Services.AddHttpClient<CatalogApiClient>((sp, http) =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CatalogApiOptions>>().Value;
    http.BaseAddress = new Uri(opts.BaseUrl);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<CorrelationIdHandler>();

builder.Services.AddHttpClient<CatalogApiClient>((sp, http) =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CatalogApiOptions>>().Value;
    http.BaseAddress = new Uri(opts.BaseUrl);
})
.AddHttpMessageHandler<CorrelationIdHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
