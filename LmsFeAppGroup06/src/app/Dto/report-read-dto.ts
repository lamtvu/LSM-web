import { UserReadDto } from "./userDto";

export interface ReportReadDTO {
    title:string;
    content: string,
    sender: UserReadDto;
    createDate: string
}
