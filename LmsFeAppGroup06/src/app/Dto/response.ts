export interface ResponseDto<T> {
    error: any;
    data: T;
    status: number;
    messager: string;
}