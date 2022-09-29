import { ContentReadDto } from "./ContentDto";

export interface SectionReadDto{
  id: number;
  name: string;
  courseId: number;
  contents: ContentReadDto[];
}

export interface SectionCreateDto{
  name: string
}
