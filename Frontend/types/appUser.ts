export interface AppUser {
    id: number,
    userName: string,
    email: string,
    profilePicture: string
    dateOfBirth: Date
    createdPredictionGames: PredictionGame[]
}