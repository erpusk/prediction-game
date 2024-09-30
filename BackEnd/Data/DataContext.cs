using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutApplication.Models.Classes;
using WorkoutApplication.Models.Enums;

namespace WorkoutApplication.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Exercise> Exercises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Exercise>().Property(x => x.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Exercise>().HasData(
            new Exercise {
                Id = 1,
                Title = "Running 800m",
                Description= "Middle distance",
                Intensity= ExerciseIntensity.Normal,
                RecommendedDurationInSeconds= 135,
                RecDurationAfter= 400,
                RecDurationBefore=100
            },
            new Exercise {
                Id = 2,
                Title = "Running 200m",
                Description= "Sprint",
                Intensity= ExerciseIntensity.High,
                RecommendedDurationInSeconds= 35,
                RecDurationAfter= 400,
                RecDurationBefore=100
            },
            new Exercise {
                Id = 3,
                Title = "Running 400m",
                Description= "Longer sprint",
                Intensity= ExerciseIntensity.High,
                RecommendedDurationInSeconds= 70,
                RecDurationAfter= 400,
                RecDurationBefore=100
            },
            new Exercise {
                Id = 4,
                Title = "Push-ups",
                Description= "Makes arms stronger",
                Intensity= ExerciseIntensity.High,
                RecommendedDurationInSeconds= 40,
                RecDurationAfter= 20,
                RecDurationBefore= 20
            },
            new Exercise {
                Id = 5,
                Title = "High-jump",
                Description= "Requires good technique",
                Intensity= ExerciseIntensity.Normal,
                RecommendedDurationInSeconds= 30,
                RecDurationAfter= 30,
                RecDurationBefore=10
            }
        );
        }
    }
}
