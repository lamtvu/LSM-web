import { RoleDto } from "./roleDto";

export interface UserReadDto {
    id: number;
    username: string;
    email: string;
    isLock: boolean;
    fullName: string;
    roleId: number;
    role: RoleDto;
    verify: boolean;
    image: string;
    gender: string;
    createDate: string;
    phone: string;
}
