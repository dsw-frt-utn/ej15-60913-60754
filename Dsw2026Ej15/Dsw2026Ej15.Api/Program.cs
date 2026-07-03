using Dsw2026Ej15.Data;
using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain;
using Microsoft.EntityFrameworkCore;
using Dsw2026Ej15.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHealthChecks(); 

// Configuración de la cadena de conexión hacia LocalDB
builder.Services.AddDbContext<Dsw2026Ej15DbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Dsw2026Ej15;Integrated Security=True"));

// Registro de la persistencia como Scoped (por cada request HTTP)
builder.Services.AddScoped<IPersistence, PersistenceEf>();

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