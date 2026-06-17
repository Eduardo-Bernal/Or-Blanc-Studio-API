using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
<<<<<<< .merge_file_pkqnz7
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrBlancAPI.Applications.Autenticacao;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.Contexts;
using OrBlancAPI.Contexts;
using OrBlancAPI.Interfaces;
using OrBlancAPI.Repositories;
using System.Text;
=======
using Microsoft.OpenApi.Models;
using OrBlancAPI.Applications.Services;
using OrBlancAPI.Contexts;
using OrBlancAPI.Interfaces;
using OrBlancAPI.Repositories;
using OrBlancAPI.Contexts;
>>>>>>> .merge_file_DrTs6J

var builder = WebApplication.CreateBuilder(args);

// carregando o .env
Env.Load();

// pegando a connection string
string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

<<<<<<< .merge_file_pkqnz7
// Conexão com banco
builder.Services.AddDbContext<OrBlancDBContext>(options => options.UseSqlServer(connectionString));
=======
builder.Services.AddDbContext<OrBlancDBContext>(
    options => options.UseSqlServer(connectionString)
);
>>>>>>> .merge_file_DrTs6J

// PROFISSIONAIS
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<ProfissionalService>();

// SERVIÇOS
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<ServicoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
<<<<<<< .merge_file_pkqnz7
builder.Services.AddSwaggerGen(c =>
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
=======
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
>>>>>>> .merge_file_DrTs6J
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
<<<<<<< .merge_file_pkqnz7
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


// Configura o sistema de autenticação da aplicação.
// Aqui estamos dizendo que o tipo de autenticação padrão será JWT Bearer.
// Ou seja: a API vai esperar receber um Token JWT nas requisições.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    // Adiciona o suporte para autenticação usando JWT.
    .AddJwtBearer(options =>
    {
        // Lê a chave secreta definida no appsettings.json.
        // Essa chave é usada para ASSINAR o token quando ele é gerado
        // e também para VALIDAR se o token recebido é verdadeiro.
        var chave = builder.Configuration["Jwt:Key"]!;

        // Quem emitiu o token (ex: nome da sua aplicação).
        // Serve para evitar aceitar tokens de outro sistema.
        var issuer = builder.Configuration["Jwt:Issuer"]!;

        // Para quem o token foi criado (normalmente o frontend ou a própria API).
        // Também ajuda a garantir que o token pertence ao seu sistema.
        var audience = builder.Configuration["Jwt:Audience"]!;

        // Define as regras que serão usadas para validar o token recebido.
                options.TokenValidationParameters = new TokenValidationParameters
        {
            // Verifica se o emissor do token é válido
            // (se bate com o issuer configurado).
            ValidateIssuer = true,

            // Verifica se o destinatário do token é válido
            // (se bate com o audience configurado).
            ValidateAudience = true,

            // Verifica se o token ainda está dentro do prazo de validade.
            // Se já expirou, a requisição será negada.
            ValidateLifetime = true,

            // Verifica se a assinatura do token é válida.
            // Isso garante que o token não foi alterado.
            ValidateIssuerSigningKey = true,

            // Define qual emissor é considerado válido.
            ValidIssuer = issuer,

            // Define qual audience é considerado válido.
            ValidAudience = audience,

            // Define qual chave será usada para validar a assinatura do token.
            // A mesma chave usada na geração do JWT deve estar aqui.
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
=======
     }

    );
>>>>>>> .merge_file_DrTs6J

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