using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrBlancAPI.Applications.Autenticacao;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.Contexts;
using OrBlancAPI.Contexts;
using OrBlancAPI.Interfaces;
using OrBlancAPI.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// carregando o .env
Env.Load();

// pegando a connection string
string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

builder.Services.AddDbContext<OrBlancDBContext>(
    options => options.UseSqlServer(connectionString)
);

// PROFISSIONAIS
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<ProfissionalService>();

// SERVI�OS
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<ServicoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
     c =>
     {
         c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
         {
             Name = "Authorization",
             Type = SecuritySchemeType.Http,
             Scheme = "bearer",
             BearerFormat = "JWT",
             In = ParameterLocation.Header,
             Description = "Value: Bearer TokenJWT"
         });
         c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

    

// Cliente
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ClienteService>();

//Profissional
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<ProfissionalService>();

//Agendamento
builder.Services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
builder.Services.AddScoped<AgendamentoService>();


// JWT
builder.Services.AddScoped<GeradorTokenJwt>();
builder.Services.AddScoped<AutenticacaoService>();

//// Content Safety
//builder.Services.AddScoped<IContentSafetyRepository, ContentSafetyService>();


// Configura o sistema de autentica��o da aplica��o.
// Aqui estamos dizendo que o tipo de autentica��o padr�o ser� JWT Bearer.
// Ou seja: a API vai esperar receber um Token JWT nas requisi��es.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    // Adiciona o suporte para autentica��o usando JWT.
    .AddJwtBearer(options =>
    {
        // L� a chave secreta definida no appsettings.json.
        // Essa chave � usada para ASSINAR o token quando ele � gerado
        // e tamb�m para VALIDAR se o token recebido � verdadeiro.
        var chave = builder.Configuration["Jwt:Key"]!;

        // Quem emitiu o token (ex: nome da sua aplica��o).
        // Serve para evitar aceitar tokens de outro sistema.
        var issuer = builder.Configuration["Jwt:Issuer"]!;

        // Para quem o token foi criado (normalmente o frontend ou a pr�pria API).
        // Tamb�m ajuda a garantir que o token pertence ao seu sistema.
        var audience = builder.Configuration["Jwt:Audience"]!;

        // Define as regras que ser�o usadas para validar o token recebido.
                options.TokenValidationParameters = new TokenValidationParameters
        {
            // Verifica se o emissor do token � v�lido
            // (se bate com o issuer configurado).
            ValidateIssuer = true,

            // Verifica se o destinat�rio do token � v�lido
            // (se bate com o audience configurado).
            ValidateAudience = true,

            // Verifica se o token ainda est� dentro do prazo de validade.
            // Se j� expirou, a requisi��o ser� negada.
            ValidateLifetime = true,

            // Verifica se a assinatura do token � v�lida.
            // Isso garante que o token n�o foi alterado.
            ValidateIssuerSigningKey = true,

            // Define qual emissor � considerado v�lido.
            ValidIssuer = issuer,

            // Define qual audience � considerado v�lido.
            ValidAudience = audience,

            // Define qual chave ser� usada para validar a assinatura do token.
            // A mesma chave usada na gera��o do JWT deve estar aqui.
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(chave)
            )
        };
    });

// Adiciona CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");


app.MapControllers();

app.Run();