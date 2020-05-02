export interface Authentication {
    user: User;
    accessToken: string;
    refreshToken: string;
}

export class User {
    id: number = -1;
    email: string = '';
}
