export class Account {
    id: number = -1;
    name: string = '';
    isPersonal: boolean = false;
    createdAt: Date = new Date();
    updatedAt?: Date;
    currentBalance: number = 0;
    openingBalance: number = 0;
    openingDate: string = '';
}

export interface GetAccountDto {
    id: number;
    name: string;
    isPersonal: boolean;
    createdAt: string;
    updatedAt?: string;
    currentBalance: number;
    openingBalance: number;
    openingDate: string;
}


export interface PostAccountDto {
    name: string;
    isPersonal: boolean;
    currentBalance: number;
    openingBalance: number;
    openingDate: string;
}
