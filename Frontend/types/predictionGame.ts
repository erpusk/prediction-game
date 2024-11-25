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
    chatMessages?: Array<{
      id: number;
      gameId: number;
      senderId: number;
      senderName?: string;
      message: string;
      timestamp: string | Date;
  }>;
  }
  