using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace itb2203_2024_predictiongame.Backend.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<PredictionGame> PredictionGames { get; set; }
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePredictionGameEntity(modelBuilder.Entity<PredictionGame>());
        ConfigureEventEntity(modelBuilder.Entity<Event>());
    }

    private static void ConfigureEventEntity(EntityTypeBuilder<Event> Event)
    {
        Event.Property(x => x.Id).ValueGeneratedOnAdd();
        Event.HasData(
            new Event
            {
                Id = 1,
                TeamA = "Koerad",
                TeamB = "Kassid",
                EventDate = new DateTime(),
                PredictionGameId = 1,
                IsCompleted = false

            }
        );
    }

    private static void ConfigurePredictionGameEntity(EntityTypeBuilder<PredictionGame> predictionGame)
    {
        predictionGame.Property(x => x.Id).ValueGeneratedOnAdd();

        predictionGame.HasData(
            new PredictionGame
            {
                Id = 1,
                PredictionGameTitle = "Football EM 2024",
                CreationDate = DateTime.Now,
                StartDate = new DateOnly(2024, 06, 14),
                EndDate = new DateOnly(2024, 07, 14),
                GameCreatorId = 1
            }
        );
    }
}
