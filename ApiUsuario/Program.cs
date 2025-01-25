using ApiUsuario.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiUsuario", Version = "v1" });
});

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuração do pipeline de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiUsuario v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Importante para registrar os endpoints

app.Run();