export interface Event {
    id: number;
    teamA: string;
    teamB: string;
    eventDate: Date;
    predictionGameId: number;
    teamAScore?: number | null;
    teamBScore?: number | null;
    isCompleted: boolean;
    // predictions?: Prediction[];  // Uncomment when implementing predictions
  }