using FinanceTracker.API.Extensions;
using FinanceTracker.Application;
using FinanceTracker.NbpRates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.AddApplicationLogic(builder.Configuration);
builder.AddNbpIntegration();

builder.Services.AddOpenApiDocumentation();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddHangfire(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Finance Tracker API");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterEndpoints();

await app.EnsureMonthlyBudgetsCreatedAsync();

app.UseHangfire();

app.Run();
