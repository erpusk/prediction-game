using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using Microsoft.VisualBasic;

namespace BackEnd.PointsCalculation
{
    public record PointsCalculator(Prediction userPrediction, Event completedEvent)
    {
        private readonly Prediction _userPrediction = userPrediction;
        private readonly Event _completedEvent = completedEvent;

            internal int CalculatePoints() {
    
            int claimedPoints = 0;
            if (_completedEvent.TeamAScore == _userPrediction.EndScoreTeamA && _completedEvent.TeamBScore == _userPrediction.EndScoreTeamB) {
                claimedPoints = 10;
            } 
            else if ((_completedEvent.TeamAScore > _completedEvent.TeamBScore && _userPrediction.EndScoreTeamA > _userPrediction.EndScoreTeamB) ||
            (_completedEvent.TeamAScore < _completedEvent.TeamBScore && _userPrediction.EndScoreTeamA < _userPrediction.EndScoreTeamB)) {
                claimedPoints = 5;
            }
            else {
                claimedPoints = 0;
            }

            return claimedPoints;
        }

    }
}