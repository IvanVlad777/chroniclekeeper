export interface AuthRequest {
    email: string;
    password: string;
}

export interface IdentityError {
    code: string;
    description: string;
}

export type UserInfo = {
    id: string;
    email: string;
    roles: string[];
};

export interface AuthContextType {
    token: string | null;
    user: DecodedToken | null;
    userInfo: UserInfo | null;
    login: (token: string) => void;
    logout: () => void;
}

export interface DecodedToken {
    sub: string; // user ID
    email: string;
    role?: string | string[]; // može biti string ili array
    jti?: string;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?:
        | string
        | string[];
    exp: number;
    iat?: number;
}
