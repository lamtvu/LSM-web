import { UserReadDto } from "./userDto";

export interface RequestStudentReadDto {
    id: number;
    senderId: number;
    sender: UserReadDto;
    classID: number;
    createDate: Date;
}

