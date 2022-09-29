import { UserReadDto } from "./userDto";

export interface CourseReadDto{
  id: number;
  name: string;
  level: number;
  isPublic: true;
  image: string;
  description: string;
  instructorId: number;
  instructor: UserReadDto;

  isLook:boolean

}


export interface CourseCreateDto{
  name: string,
  level: number,
  isPublic: true,
  description: string
}
