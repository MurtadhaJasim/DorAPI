using AutoMapper;
using Dor;
using Dor.Data;
using Dor.Filters;
using Dor.Interfaces;
using Dor.Mappings;
using Dor.Repository;
using Dor.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================
// 1. الإعدادات العامة
// =======================
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddLogging(cfg => cfg.AddDebug());
builder.Services.AddResponseCompression();

// =======================
// 2. إعداد CORS
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =======================
// 3. الفلاتر والتحكم
// =======================
builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogActivityFilter>();
});

// =======================
// 4. الخدمات العامة
// =======================
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IAuthService, AuthService>();

// =======================
// 5. FileService
// =======================
builder.Services.AddScoped<FileService>(provider =>
{
    var fileStoragePath = builder.Configuration["FileStoragePath"] ?? "wwwroot/files";
    return new FileService(fileStoragePath);
});

// =======================
// 6. Swagger مع توثيق JWT
// =======================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });

    options.CustomOperationIds(apiDesc =>
        $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}_{apiDesc.RelativePath}");
});

// =======================
// 7. إعداد قاعدة البيانات
// =======================
string? databaseUrl = builder.Configuration["DATABASE_URL"];
string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    connectionString = ConvertPostgresUrlToConnectionString(databaseUrl);
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// =======================
// 8. إعداد JWT
// =======================
builder.Services.Configure<jwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<jwtOptions>();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Signingkey)),
            ValidateLifetime = true
        };
    });

// =======================
// 9. إعداد المنفذ من Railway
// =======================
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// =======================
// 10. بناء التطبيق
// =======================
var app = builder.Build();

// =======================
// 11. التهيئة وقت التشغيل
// =======================
app.UseResponseCompression();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //db.Database.Migrate(); // ✅ تنفيذ المايغريشن
}

// =======================
// 12. ملفات ثابتة
// =======================
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});

// =======================
// 13. Middlewares
// =======================
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// =======================
// 14. تشغيل Controllers
// =======================
app.MapControllers();

app.Run();

// =======================
// 15. تحويل DATABASE_URL
// =======================
static string ConvertPostgresUrlToConnectionString(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    var builder = new Npgsql.NpgsqlConnectionStringBuilder()
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = Npgsql.SslMode.Prefer,
        TrustServerCertificate = true
    };

    return builder.ToString();
}
