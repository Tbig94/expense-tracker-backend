using ExpenseTrackerApi.Application.Budgets.Commands.CreateBudget;
using ExpenseTrackerApi.Application.Budgets.Commands.DeleteBudget;
using ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOverflow;
using ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOVerflow;
using ExpenseTrackerApi.Application.Categories.Commands.CreateCategory;
using ExpenseTrackerApi.Application.Categories.Commands.DeleteCategory;
using ExpenseTrackerApi.Application.Categories.Queries.GetCategory;
using ExpenseTrackerApi.Application.Common.Behaviours;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Expenses.Commands.CreateExpense;
using ExpenseTrackerApi.Application.Expenses.Commands.DeleteExpense;
using ExpenseTrackerApi.Application.Statistics.Queries.MonthlyStatistics;
using ExpenseTrackerApi.Application.Statistics.Queries.YearlyStatistics;
using ExpenseTrackerApi.Infrastructure.Persistence;
using ExpenseTrackerApi.Infrastructure.Services;
using ExpenseTrackerApi.Web.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    }));

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Microsoft.Extensions.Diagnostics.HealthChecks.SqlServer


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<ICsvExportService, CsvExportService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)
            ),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };
    });

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Beállítások beolvasása appsettings.json-ból
    .Enrich.FromLogContext()
    //.WriteTo.MSSqlServer(
    //    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
    //    sinkOptions: new MSSqlServerSinkOptions
    //    {
    //        SchemaName = "log",
    //        TableName = "HttpLogs",
    //        AutoCreateSqlTable = true,
    //        BatchPostingLimit = 50,              // Kötegekben küldi a logokat
    //        BatchPeriod = TimeSpan.FromSeconds(5) // Max 5 másodpercenként ír az SQL-be
    //    })
    .WriteTo.Console() // fallback, ha SQL nem elérhető
    .CreateLogger();

builder.Host.UseSerilog();

ConfigureCors(builder);

ConfigureValidators(builder);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

ConfigureMediator(builder);


var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} (User: {UserId}) responded {StatusCode} in {Elapsed:0} ms";

    // 1. KIVÉTELEK KEZELÉSE (Auth végpontok elnyomása)
    options.GetLevel = (httpContext, elapsed, ex) =>
    {
        // Ha hiba/kivétel történt, azt mindenképp logoljuk Error szinten
        if (ex != null || httpContext.Response.StatusCode >= 500)
        {
            return LogEventLevel.Error;
        }

        // Ellenőrizzük, hogy az URL elérési útja tartalmazza-e az /auth/ részt
        var requestPath = httpContext.Request.Path.Value;
        if (requestPath != null && requestPath.StartsWith("/api/Auth", StringComparison.OrdinalIgnoreCase))
        {
            // Verbose vagy Debug szintűre állítjuk, amit az appsettings.json minimum szintje figyelmen kívül hagy
            return LogEventLevel.Verbose;
        }

        // Minden más normál kérés marad Information szintű
        return LogEventLevel.Information;
    };

    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        var userId = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                     ?? httpContext.User.Identity?.Name
                     ?? "Anonymous";

        diagnosticContext.Set("UserId", userId);
    };
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<JwtAuthenticationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<DevelopmentAuthenticationMiddleware>();
}

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler("/error");

app.MapHealthChecks("/health");

app.Run();


static void ConfigureMediator(WebApplicationBuilder builder)
{
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(ExpenseTrackerApi.Application.AssemblyReference).Assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
}

static void ConfigureCors(WebApplicationBuilder builder)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

static void ConfigureValidators(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IValidator<CreateBudgetCommand>, CreateBudgetCommandValidator>();
    builder.Services.AddScoped<IValidator<DeleteBudgetCommand>, DeleteBudgetCommandValidator>();
    builder.Services.AddScoped<IValidator<CheckBudgetOverflowQuery>, CheckBudgetOverflowQueryValidator>();

    builder.Services.AddScoped<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
    builder.Services.AddScoped<IValidator<DeleteCategoryCommand>, DeleteCategoryCommandValidator>();
    builder.Services.AddScoped<IValidator<GetCategoryQuery>, GetCategoryQueryValidator>();

    builder.Services.AddScoped<IValidator<CreateExpenseCommand>, CreateExpenseCommandValidator>();
    builder.Services.AddScoped<IValidator<DeleteExpenseCommand>, DeleteExpenseCommandValidator>();

    builder.Services.AddScoped<IValidator<MonthlyStatisticsQuery>, MonthlyStatisticsQueryValidator>();
    builder.Services.AddScoped<IValidator<YearlyStatisticsQuery>, YearlyStatisticsQueryValidator>();
}