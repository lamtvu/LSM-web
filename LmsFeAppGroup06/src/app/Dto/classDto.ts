import { UserReadDto } from "./userDto";

export interface ClassReadDto {
    id: number;
    teacherId: number;
    teacher: UserReadDto;
    classAdmin: UserReadDto;
    assistant: UserReadDto;
    name: string;
    description: string;
    createDate: string;
}