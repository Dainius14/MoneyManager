export interface Authentication {
    user: User;
    accessToken: string;
    refreshToken: string;
}

export interface User {
    id: number;
    email: string;
}
