using FinanceTracker.API.Extensions;
using FinanceTracker.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.AddApplicationLogic(builder.Configuration);

builder.Services.AddValidators();

builder.Services.AddOpenApiDocumentation();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddHangfire(builder.Configuration);

builder.Services.AddCorsPolicies();

var app = builder.Build();

app.UseCors("FrontendClient");

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

app.Seed();

app.Run();