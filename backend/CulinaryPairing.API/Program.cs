using System.Text;
using CulinaryPairing.BLL.Services;
using CulinaryPairing.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CulinaryPairing.BLL.PairingEngine;
using CulinaryPairing.BLL.PairingEngine.Rules;
using CulinaryPairing.BLL.VideFrigo;
using CulinaryPairing.BLL.Substitution;
using CulinaryPairing.BLL.Favoris;
using CulinaryPairing.BLL.Contraintes;
using CulinaryPairing.BLL.Soirees;
using CulinaryPairing.BLL.Historique;

var builder = WebApplication.CreateBuilder(args);

// SERVICES

// ----- DbContext EF Core -----
builder.Services.AddDbContext<CulinaryPairingDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ----- Services métier (Dependency Injection) -----
builder.Services.AddScoped<IAuthService, AuthService>();

// ----- Authentification JWT -----
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key manquante dans la configuration.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("Jwt:Issuer manquant.");
var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("Jwt:Audience manquant.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero // pas de tolérance sur l'expiration
    };
});

builder.Services.AddAuthorization();

// === Moteur d'accords (CdC v1.3 §2.5) ===
builder.Services.AddScoped<IPairingEngineService, PairingEngineService>();

// Règles d'accord (Strategy pattern) - injectées en IEnumerable<IPairingRule> dans le moteur
builder.Services.AddScoped<IPairingRule, R10bis_IntensiteAromatique>();
builder.Services.AddScoped<IPairingRule, R10_AciditeGras>();
builder.Services.AddScoped<IPairingRule, R14bis_AciditeEquivalente>();
builder.Services.AddScoped<IPairingRule, R14_LegereFraicheur>();
builder.Services.AddScoped<IPairingRule, R11_PiquantDoux>();
builder.Services.AddScoped<IPairingRule, R11bis_SucreDessert>();
builder.Services.AddScoped<IPairingRule, R24bis_SucreSaleHorsDessert>();
builder.Services.AddScoped<IPairingRule, R12_UmamiPurTannins>();
builder.Services.AddScoped<IPairingRule, R13bis_AffiniteTannins>();
builder.Services.AddScoped<IPairingRule, R13_FumeTannique>();
builder.Services.AddScoped<IPairingRule, R20bis_CuissonTanninsStructures>();
builder.Services.AddScoped<IPairingRule, R21bis_AccordSauce>();
builder.Services.AddScoped<IPairingRule, R22bis_Amertume>();
builder.Services.AddScoped<IPairingRule, R19bis_SimilitudeAromatique>();
builder.Services.AddScoped<IPairingRule, R23bis_SelTannins>();
builder.Services.AddScoped<IPairingRule, R25bis_AromesEpices>();

// ----- CORS (autoriser Angular à appeler l'API) -----
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----- Controllers + OpenAPI -----
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IAccordsService, AccordsService>();
builder.Services.AddScoped<IBoissonsService, BoissonsService>();
builder.Services.AddScoped<VideFrigoService>();
builder.Services.AddScoped<ISubstitutionService, SubstitutionService>();
builder.Services.AddScoped<IFavorisService, FavorisService>();
builder.Services.AddScoped<IContraintesService, ContraintesService>();
builder.Services.AddScoped<ISoireesService, SoireesService>();
builder.Services.AddScoped<IMenusService, MenusService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IHistoriqueService, HistoriqueService>();

var app = builder.Build();

// ===== Seed de la BDD au démarrage =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CulinaryPairingDbContext>();
    await CulinaryPairing.DAL.Seed.DbInitializer.SeedAsync(db);
}

// PIPELINE HTTP

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AngularDev");
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();

app.Run();