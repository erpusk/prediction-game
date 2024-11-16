using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
using Microsoft.VisualBasic;

namespace BackEnd.PointsCalculation
{
    public class PointsCalculator
    {
            internal int CalculatePoints(Event completedEvent, Prediction userPrediction) {
    
            int claimedPoints = 0;
            if (completedEvent.TeamAScore == userPrediction.EndScoreTeamA && completedEvent.TeamBScore == userPrediction.EndScoreTeamB) {
                claimedPoints = 10;
            } 
            else if ((completedEvent.TeamAScore > completedEvent.TeamBScore && userPrediction.EndScoreTeamA > userPrediction.EndScoreTeamB) ||
            (completedEvent.TeamAScore < completedEvent.TeamBScore && userPrediction.EndScoreTeamA < userPrediction.EndScoreTeamB)) {
                claimedPoints = 5;
            }
            else {
                claimedPoints = 0;
            }

            return claimedPoints;
        }

    }
}