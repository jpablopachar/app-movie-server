using System.Text;
using app_movie_server.Data;
using app_movie_server.Models;
using app_movie_server.Profiles;
using app_movie_server.Repositories;
using app_movie_server.Repositories.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<IMovieRepository, MovieRepository>();
    services.AddScoped<IUserRepository, UserRepository>();

    // Configuración Identity
    services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

    // Configuración de autenticación JWT
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]!)),
                ValidIssuer = configuration["Token:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = false
            };
        });

    // Configuración de AutoMapper
    services.AddAutoMapper(typeof(MappingProfile));

    // Soporte para Caché
    var apiVersionBuilder = services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    });

    apiVersionBuilder.AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    // Soporte para versionamiento
    services.AddApiVersioning();

    // Configuración de DbContext
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SQLServerConnection")));

    // Controladores
    services.AddControllers(options =>
    {
        options.CacheProfiles.Add("Default30Seconds", new CacheProfile()
        {
            Duration = 30
        });
    });

    // Configuración de Cors
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("*");
        });
    });

    // Configuración de Swagger
    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(options =>
    {
        // Configuración para resolver conflictos de nombres de esquema
        options.CustomSchemaIds(type => type.FullName);

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
            "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
            "Ingresa la palabra 'Bearer' seguido de un [espacio] y después su token en el campo de abajo.\r\n\r\n" +
            "Ejemplo: \"Bearer tkljk125jhhk\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1.0",
            Title = "Movies Api V1",
            Description = "Api de Movies Versión 1",
            TermsOfService = new Uri("https://render2web.com/promociones"),
            Contact = new OpenApiContact
            {
                Name = "render2web",
                Url = new Uri("https://render2web.com/promociones")
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://render2web.com/promociones")
            }
        });

        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2.0",
            Title = "Movies Api V2",
            Description = "Api de Movies Versión 2",
            TermsOfService = new Uri("https://render2web.com/promociones"),
            Contact = new OpenApiContact
            {
                Name = "render2web",
                Url = new Uri("https://render2web.com/promociones")
            },
            License = new OpenApiLicense
            {
                Name = "Licencia Personal",
                Url = new Uri("https://render2web.com/promociones")
            }
        });
    });
}

static void ConfigurePipeline(WebApplication app)
{
    // Configuración de Swagger - DEBE IR PRIMERO
    app.UseSwagger();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesApiV1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "MoviesApiV2");
        });
    }
    else
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesApiV1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "MoviesApiV2");
            options.RoutePrefix = "";
        });
    }

    // Redirección HTTPS
    app.UseHttpsRedirection();

    // Soporte para archivos estáticos
    app.UseStaticFiles();

    // CORS
    app.UseCors("CorsPolicy");

    // Autenticación y Autorización
    app.UseAuthentication();
    app.UseAuthorization();

    // Mapeo de controladores
    app.MapControllers();
}