export interface AnswerReadDto {
    id: number;
    content: string;
    isCorrect: boolean;
    questionId: number;
}
export interface AnswerCreateDto {
    content: string;
    isCorrect: boolean;
}

