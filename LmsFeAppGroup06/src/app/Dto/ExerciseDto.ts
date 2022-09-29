import { ClassReadDto } from "./classDto";

export interface ExerciseReadDto {
    id: number;
    name: string;
    description: string;
    createDate: string;
    dueDate: string;
    classReadDto: ClassReadDto;
    classId: number;
}

export interface ExerciseCreateDto {
    name: string;
    description: string;
    createDate: string;
    dueDate: Date;
}