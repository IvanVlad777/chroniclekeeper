import { useState } from "react";
import { AuthContext } from "./AuthContext";
import { DecodedToken, UserInfo } from "../../interfaces/authInterfaces";
import { jwtDecode } from "jwt-decode";

//helper: pretvori bilo koji oblik role claimova u string[]
function normalizeRoles(decoded?: DecodedToken): string[] {
    if (!decoded) return [];
    const raw =
        decoded.role ??
        decoded[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ] ??
        [];
    return Array.isArray(raw) ? raw : raw ? [raw] : [];
}

// helper: iz DecodedToken složi praktični objekt za UI
function toUserInfo(decoded: DecodedToken | null): UserInfo | null {
    if (!decoded) return null;
    return {
        id: decoded.sub || "",
        email: decoded.email || "",
        roles: normalizeRoles(decoded),
    };
}

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [token, setToken] = useState<string | null>(
        sessionStorage.getItem("token")
    );

    const [user, setUser] = useState<DecodedToken | null>(() => {
        const stored = sessionStorage.getItem("token");
        if (stored) {
            try {
                return jwtDecode<DecodedToken>(stored);
            } catch {
                return null;
            }
        }
        return null;
    });

    const userInfo: UserInfo | null = toUserInfo(user);

    const login = (token: string) => {
        sessionStorage.setItem("token", token);
        setToken(token);
        const decoded = jwtDecode<DecodedToken>(token);
        setUser(decoded);
    };

    const logout = () => {
        sessionStorage.removeItem("token");
        setToken(null);
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ token, user, userInfo, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};
