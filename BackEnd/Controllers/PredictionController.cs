using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data.Repos;
using BackEnd.Mappers;
using BackEnd.Models.Classes;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController: ControllerBase
    {
        private readonly PredictionRepo _repo;
        public PredictionController(PredictionRepo repo){
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll (){
            var result = await _repo.GetAll();
            var resultAsDto = result.Select(x => x.toPredictionDto()).ToList();
            return Ok(result);
        }
        
    }
}