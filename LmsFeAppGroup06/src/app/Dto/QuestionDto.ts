import { AnswerReadDto } from "./AnswerDto";

export interface QuestionReadDto {
    id: number,
    content: string;
    answers: QuestionReadDto[];
}

export interface QuestionCreateDto {
    content: string,
    answers: AnswerReadDto[];
}