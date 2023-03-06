using EventManager.Domain.Events;
using EventManager.Persistence.Repositories;
using EventManagerService.Extensions;
using EventManagerService.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISocialEventRepository, SocialEventRepository>();

builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddJWTBearerAuthentication(builder.Configuration);
builder.Services.AddApiScopeAuthorization();

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

app.MapControllers()
    .RequireAuthorization(AuthorizationPolicy.ApiScope().Name);

app.Run();


