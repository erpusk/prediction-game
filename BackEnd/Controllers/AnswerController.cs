using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnswerController: ControllerBase
    {
        private readonly AnswerRepo _repo;

        public AnswerController(AnswerRepo repo){
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnswers([FromQuery]int? questionId){
            var result = await _repo.GetAnswers(questionId);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAnswer(int Id){
            var result = await _repo.GetAnswer(Id);
            if (result == null){
                return NotFound("Answer was not found");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAnswer([FromBody]Answer answerModel){
            var answerExists = await _repo.GetAnswer(answerModel.Id);
            if (answerExists != null){
                return BadRequest("Answer already exists");
            }

            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId)) {
                return Unauthorized("User ID claim not found.");
            }

            answerModel.AnswerMakerId =  userId;
            var result = await _repo.AddAnswer(answerModel);
            return CreatedAtAction(nameof(GetAnswer), new {Id = answerModel.Id}, answerModel);
        }

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetAnswerByQuestionId(int questionId){
            var result = await _repo.GetAnswerByQuestionId(questionId);
            if (result == null){
                return NotFound("Answer was not found");
            }
            return Ok(result);
        }

        [HttpGet("user/question/{questionid}")]
        public async Task<IActionResult> GetByQuestionAndUserId(int questionid){
            try {
            var userIdClaim = int.Parse(User.FindFirst("userId")!.Value);
            var resultAsList = await _repo.GetAnswers(questionid, userIdClaim);
            if (resultAsList == null) {
                return NotFound("Predictions, with your event id or userid were not found");
            }
            var result = resultAsList[0];
            return Ok(result);
            } catch {
                return Ok(null);
            }
        }
    
    }
}