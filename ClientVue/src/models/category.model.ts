export class Category {
    id: number = -1;
    name: string = '';
    createdAt: Date = new Date();
    updatedAt?: Date;
    parent?: Category;
}

export interface GetCategoryDto {
    id: number;
    name: string;
    createdAt: string;
    updatedAt?: string;
    parent?: GetCategoryDto;
}


export interface PostCategoryDto {
    name: string;
    parent?: PostCategoryDto;
}
