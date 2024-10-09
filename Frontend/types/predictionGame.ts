export interface PredictionGame {
    id: number;
    predictionGameTitle: string;
    creationDate: Date;
    startDate: string | Date | undefined;
    endDate: string | Date | undefined;
    gameCreatorId: number;
    privacy: string;
    //Events: GameEvent[];
    // participants?: GameParticipant[];  // Uncomment when implementing participants
  }
  