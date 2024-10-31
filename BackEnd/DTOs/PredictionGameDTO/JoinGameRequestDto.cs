using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.DTOs.PredictionGameDTO;

namespace BackEnd.DTOs.PredictionGameDTO;

public class JoinGameRequestDto
{
    public string? UniqueCode { get; set; }
    public int UserId { get; set; }
}
