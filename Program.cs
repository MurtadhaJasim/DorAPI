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

// تحميل متغيرات البيئة (Railway أو أي منصة)
builder.Configuration.AddEnvironmentVariables();

// تسجيل الخدمات والاعتمادات
builder.Services.AddLogging(cfg => cfg.AddDebug());
builder.Services.AddResponseCompression();
builder.Services.AddControllers(options => options.Filters.Add<LogActivityFilter>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<FileService>(provider =>
{
    var path = builder.Configuration["FileStoragePath"] ?? "wwwroot/files";
    return new FileService(path);
});

// إعداد CORS للفرونت من Netlify
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("https://dor-complix.netlify.app")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Swagger مع دعم JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Token"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            } }, Array.Empty<string>() }
    });
});

// إعداد قاعدة البيانات PostgreSQL
// استخدم متغير البيئة DATABASE_PUBLIC_URL مع التحويل إلى سلسلة اتصال Npgsql صحيحة
string? databaseUrl = builder.Configuration["DATABASE_PUBLIC_URL"];

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

// إعداد JWT
builder.Services.Configure<jwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<jwtOptions>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Signingkey)),
            ValidateLifetime = true
        };
    });

// إعداد المنفذ من متغيرات البيئة
var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, int.Parse(port));
});

var app = builder.Build();

// تنفيذ المايكريشن التلقائي عند تشغيل التطبيق
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Middleware
app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dor API V1");
    c.RoutePrefix = string.Empty; // يجعل Swagger في صفحة الجذر
});


app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

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
