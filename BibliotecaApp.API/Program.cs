using BibliotecaApp.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. ÁREA DE CONFIGURAÇÃO (Serviços)
// ==========================================

// Configura a conexão com o PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o suporte a Controllers (que vamos criar depois)
builder.Services.AddControllers();

// Configura o Swagger (Documentação automática)
// Se você estiver no .NET 9, pode manter o AddOpenApi, mas o SwaggerGen é o clássico
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ==========================================
// 2. ÁREA DE EXECUÇÃO (Pipeline)
// ==========================================

// Configura o Swagger para aparecer no navegador
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTudo");

// Diz para a API usar os Controllers que criarmos
app.MapControllers(); 

app.Run();