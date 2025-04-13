export interface AuthRequest {
  email: string;
  password: string;
}

export interface IdentityError {
  code: string;
  description: string;
}

export interface AuthContextType {
  token: string | null;
  user: DecodedToken | null;
  login: (token: string) => void;
  logout: () => void;
}

export interface DecodedToken {
  sub: string; // user ID
  email: string;
  role: string | string[]; // može biti string ili array
  exp: number;
  iat?: number;
}
