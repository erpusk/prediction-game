export interface ChatMessage {
    id: number;
    gameId: number;
    senderName?: string;
    message?: string;
    timestamp: string;
}