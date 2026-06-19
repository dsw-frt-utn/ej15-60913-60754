using Dsw2026Ej15.Data;
using Dsw2026Ej15.Data.Persistance;
using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHealthChecks(); 


builder.Services.AddSingleton<IPersistence, PersistenceInMemory>(); 

var app = builder.Build();


app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400; 
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception)
    {
        context.Response.StatusCode = 500; 
        await context.Response.WriteAsJsonAsync(new { error = "Problem" });
    }
});

app.UseRouting();
app.MapControllers();


app.MapHealthChecks("/health-check");

app.Run();