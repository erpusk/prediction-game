export type BonusQuestionType = 'String' | 'Number' | 'MultipleChoice';

export interface BonusQuestion{
    id: number,
    predictionGameId: number,
    question: string,
    questionType: BonusQuestionType,
    optionsJson?: string,
}