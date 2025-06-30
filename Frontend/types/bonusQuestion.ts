export type BonusQuestionType = 'StringQuestion' | 'NumberQuestion' | 'MultipleChoiceQuestion';

export interface BonusQuestion{
    id: number,
    predictionGameId: number,
    question: string,
    questionType: BonusQuestionType,
    correctAnswer: string,
    options: string[],
}