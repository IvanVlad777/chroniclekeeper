
// util koji iz JWT-a vadi email i role (podržava oba claim tipa)
import { jwtDecode } from "jwt-decode";

type Decoded = {
    sub?: string;
    email?: string;
    role?: string | string[]; // plain "role"
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string | string[]; // URI role
};

export function parseUserFromToken(token: string) {
    const d = jwtDecode<Decoded>(token);

    const rolesRaw =
        d.role ??
        d["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ??
        [];

    const roles = Array.isArray(rolesRaw) ? rolesRaw : rolesRaw ? [rolesRaw] : [];

    return {
        id: d.sub || "",
        email: d.email || "",
        role: roles, // uvijek string[]
    };
}
