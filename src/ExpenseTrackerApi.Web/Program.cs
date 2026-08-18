using ExpenseTrackerApi.Application.Budgets.Commands.CreateBudget;
using ExpenseTrackerApi.Application.Budgets.Commands.DeleteBudget;
using ExpenseTrackerApi.Application.Budgets.Commands.EditBudget;
using ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOverflow;
using ExpenseTrackerApi.Application.Budgets.Queries.CheckBudgetOVerflow;
using ExpenseTrackerApi.Application.Budgets.Queries.GetBudgets;
using ExpenseTrackerApi.Application.Categories.Commands.CreateCategory;
using ExpenseTrackerApi.Application.Categories.Commands.DeleteCategory;
using ExpenseTrackerApi.Application.Categories.Queries.GetCategory;
using ExpenseTrackerApi.Application.Common.Behaviours;
using ExpenseTrackerApi.Application.Common.Interfaces;
using ExpenseTrackerApi.Application.Expenses.Commands.CreateExpense;
using ExpenseTrackerApi.Application.Expenses.Commands.DeleteExpense;
using ExpenseTrackerApi.Application.Expenses.Commands.UpdateExpense;
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
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
                Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
            ),
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
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


ConfigureCors(builder);

ConfigureValidators(builder);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

ConfigureMediator(builder);


var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

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
    // Manuálisan regisztráld az összes validátort
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

    // ... stb. a többi validátor
}