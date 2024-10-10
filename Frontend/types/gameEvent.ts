export interface GameEvent {
    id: number;
    teamA: string;
    teamB: string;
    eventDate: Date | string | undefined;
    predictionGameId: number;
    teamAScore: number;
    teamBScore: number;
    isCompleted: boolean;
    // predictions?: Prediction[];  // Uncomment when implementing predictions
  }