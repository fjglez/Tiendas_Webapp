using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Practica.TiendasAPI.DbContexts;
using Practica.TiendasAPI.Services;
using Serilog;
using System.Reflection;


// Añadir logs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/logstienda.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;  // Devuelve 406 si no soporta el formato
}).AddNewtonsoftJson(); // Patch

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
});

// Configurando la DB
builder.Services.AddDbContext<TiendaContext>( // Usando SQLite
    dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:DBConnectionString"]));
builder.Services.AddDefaultIdentity<IdentityUser>(/* Opciones si fuesen necesarias*/)
                .AddEntityFrameworkStores<TiendaContext>();

builder.Services.AddScoped<ITiendaRepository, TiendaRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Integracion de API con Angular
builder.Services.AddCors();

var app = builder.Build();

/* .WithOrigins("http://localhost:4200") */
app.UseCors(x => x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
