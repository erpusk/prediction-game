using BackEnd.Models.Classes;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RandomString4Net;

namespace itb2203_2024_predictiongame.Backend.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<PredictionGame> PredictionGames { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers{ get; set; }
    public DbSet<Prediction> Predictions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePredictionGameEntity(modelBuilder.Entity<PredictionGame>());
        ConfigureEventEntity(modelBuilder.Entity<Event>());
        ConfigureApplicationUserEntity(modelBuilder.Entity<ApplicationUser>());
        ConfigurePredictionEntity(modelBuilder.Entity<Prediction>());
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
                EventDate = DateTime.Now.ToUniversalTime(),
                PredictionGameId = 1,
                IsCompleted = false

            }
        );
    }

    private static void ConfigurePredictionGameEntity(EntityTypeBuilder<PredictionGame> predictionGame)
    {

        predictionGame
        .HasOne(e => e.GameCreator)
        .WithMany(e => e.CreatedPredictionGames)
        .HasForeignKey(e => e.GameCreatorId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
        
        predictionGame.Property(x => x.Id).ValueGeneratedOnAdd();

        predictionGame.HasData(
            new PredictionGame
            {
                Id = 1,
                PredictionGameTitle = "Football EM 2024",
                CreationDate = new DateTime(2024, 06, 14).ToUniversalTime(),
                StartDate = new DateTime(2024, 06, 14).ToUniversalTime(),
                EndDate = new DateTime(2024, 07, 14).ToUniversalTime(),
                GameCreatorId = 1,
                UniqueCode =  RandomString.GetString(Types.ALPHABET_LOWERCASE, 6),
            }
        );
    }
    private static void ConfigureApplicationUserEntity(EntityTypeBuilder<ApplicationUser> user)
    {
        user.Property(x => x.Id).ValueGeneratedOnAdd();
        user.HasData(
            new ApplicationUser
            {
                Id = 1,
                UserName = "MariMas",
                DateOfBirth = DateTime.Now.ToUniversalTime(),
            }
        );
    }

    private static void ConfigurePredictionEntity(EntityTypeBuilder<Prediction> prediction){
        prediction.Property(x => x.Id).ValueGeneratedOnAdd();
        prediction.HasData(
            new Prediction{
                Id = 1,
                EndScoreTeamA = 2,
                EndScoreTeamB = 6,
                PredictionMakerId = 1,
                EventId = 1
            }
        );

    }
}
