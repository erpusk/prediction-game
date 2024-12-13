using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Classes
{
    public class BonusQuestion
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public int PredictionGameId { get; set; }
    }
}