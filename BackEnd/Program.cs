using Microsoft.EntityFrameworkCore;
using itb2203_2024_predictiongame.Backend.Data.Repos;
using itb2203_2024_predictiongame.Backend.Data;
using BackEnd.Data.Repos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddControllers().AddNewtonsoftJson(options =>{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<PredictionGamesRepo>()
    .AddScoped<EventRepo>()
    .AddScoped<ApplicationUserRepo>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    }));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("MyPolicy");
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
