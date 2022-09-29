import { AnswerReadDto } from "./AnswerDto";
import { ExerciseReadDto } from "./ExerciseDto";
import { QuizReadDto } from "./QuizDto";
import { UserReadDto } from "./userDto";

export interface SubmissionExerciseReadDto {
    id: number;
    core: number;
    comment: string;
    fileType: string;
    submitDate: Date;
    exerciseId: number;
    exercise: ExerciseReadDto;
    student: UserReadDto;
}

export interface SubmissionQuizReaDto {
    id: number;
    core: number;
    comment: string;
    startTime: string;
    finishTime: string;
    isFinish: boolean;
    quizId: number;
    studentId: number;
    student: UserReadDto;
    answers: AnswerReadDto[];
    quiz: QuizReadDto;
}