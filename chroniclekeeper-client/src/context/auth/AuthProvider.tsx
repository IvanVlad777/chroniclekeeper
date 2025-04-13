import { useState } from "react";
import { AuthContext } from "./AuthContext";
import { DecodedToken } from "../../interfaces/authInterfaces";
import { jwtDecode } from "jwt-decode";

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

  const login = (token: string) => {
    sessionStorage.setItem("token", token);
    setToken(token);
    setUser(jwtDecode<DecodedToken>(token));
  };

  const logout = () => {
    sessionStorage.removeItem("token");
    setToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ token, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
