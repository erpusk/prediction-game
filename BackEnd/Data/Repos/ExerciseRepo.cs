using Microsoft.EntityFrameworkCore;
using WorkoutApplication.Models.Classes;
using WorkoutApplication.Models.Enums;

namespace WorkoutApplication.Data.Repos
{
    public class ExercisesRepo(DataContext context)
    {
        private readonly DataContext context = context;
        
        //CREATE
        public async Task<Exercise> SaveExerciseToDb(Exercise exercise) {
            context.Add(exercise);
            await context.SaveChangesAsync();
            return exercise;
        }

        //READ
        public async Task<List<Exercise>> GetAllExercises(ExerciseIntensity? intensity = null ) {
            IQueryable<Exercise> query = context.Exercises.AsQueryable();
            if (intensity.HasValue) {
                query = query.Where(x => x.Intensity == intensity);
            }
            
            return await query.ToListAsync();
        }
        public async Task<Exercise?> GetExerciseById(int id) => await context.Exercises.FindAsync(id);
        public async Task<bool> ExerciseExistsInDb(int id) => await context.Exercises.AnyAsync(x => x.Id == id);

        //UPDATE 
        public async Task<bool> UpdateExercise(int id, Exercise exercise) {

            bool isIdsMatch = id == exercise.Id;
            bool exerciseExists = await ExerciseExistsInDb(id);

            if(!isIdsMatch || !exerciseExists){
                return false;
            }

            context.Update(exercise);
            int updatedRecordsCount = await context.SaveChangesAsync();
            return updatedRecordsCount == 1;
        }

        //DELETE
        public async Task<bool> DeleteExerciseById(int id) {
            Exercise? exerciseInDb = await GetExerciseById(id);
            if (exerciseInDb is null){
                return false;
            }
            context.Remove(exerciseInDb);
            int changesCount = await context.SaveChangesAsync();

            return changesCount == 1;
        }
        
    }
}
