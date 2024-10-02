using itb2203_2024_predictiongame.Backend.Data.Repos;
using itb2203_2024_predictiongame.Backend.Models.Classes;
using itb2203_2024_predictiongame.Backend.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace itb2203_2024_predictiongame.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExercisesController(ExercisesRepo repo) : ControllerBase()
    {
        private readonly ExercisesRepo repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ExerciseIntensity? intensity)
        {
            var result = await repo.GetAllExercises(intensity);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExercise(int id)
        {
            var exercise = await repo.GetExerciseById(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> SaveExercise([FromBody] Exercise exercise)
        {
            var exerciseExists = await repo.ExerciseExistsInDb(exercise.Id);
            if (exerciseExists)
            {
                return Conflict();
            }
            var result = await repo.SaveExerciseToDb(exercise);
            return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Exercise exercise)
        {
            bool result = await repo.UpdateExercise(id, exercise);
            return result ? NoContent() : NotFound();
        }


    }
}
