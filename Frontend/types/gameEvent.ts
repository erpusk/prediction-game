export interface GameEvent {
    id: number;
    teamA: string;
    teamB: string;
    eventDate: Date | string | undefined;
    predictionGameId: number;
    teamAScore?: number | null;
    teamBScore?: number | null;
    isCompleted: boolean;
    // predictions?: Prediction[];  // Uncomment when implementing predictions
  }