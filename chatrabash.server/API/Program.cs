using Application.Core;
using Application.Rooms.Queries;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
   opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(x => {
    x.RegisterServicesFromAssemblyContaining<GetAllRooms.Handler>();
    x.AddOpenBehavior(typeof(ValidatorBehavior<,>));
});

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfiles).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<Application.Rooms.Validators.Create>();

builder.Services.AddTransient<API.Middleware.ExceptionMiddleware>();
builder.Services.AddIdentityApiEndpoints<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

app.UseMiddleware<API.Middleware.ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapGroup("api").MapIdentityApi<User>();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration.");
}

app.Run();
