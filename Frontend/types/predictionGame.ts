export interface PredictionGame {
    id: number;
    predictionGameTitle: string;
    creationDate: Date | string;
    startDate: string | Date | undefined;
    endDate: string | Date | undefined;
    gameCreatorId: number;
    privacy: string;
    uniqueCode: string;
    participants: Array<{ id: number; userId: number; userName: string }>;
    //Events: GameEvent[];
    participants?: GameParticipant[];  // Uncomment when implementing participants
  }
  