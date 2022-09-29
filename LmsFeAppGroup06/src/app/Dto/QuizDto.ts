export interface QuizCreateDto {
    name: string;
    description: string;
    startDate: string;
    startTime: string;
    duration: number;
}

export interface QuizReadDto {
    id: number;
    name: string;
    description: string;
    startDate: string;
    duration: number;
    createDate: string;
    classId: number;
}