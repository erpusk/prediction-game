export interface PredictionGame {
    id: number;
    predictionGameTitle: string;
    creationDate: Date;
    startDate: string | Date | undefined;
    endDate: string | Date | undefined;
    gameCreatorId: number;
    privacy: string;
    //events: Event[];
    // participants?: GameParticipant[];  // Uncomment when implementing participants
  }
  