import { UserReadDto } from "./userDto";

export interface CommentReadDto{
  id: number;
  star: number;
  comment: string;
  createDate: string;
  courseId: number;
  senderId: number;
  sender:UserReadDto;
}


export interface CommentCreateDto{
  star: number;
  comment: string;
}
