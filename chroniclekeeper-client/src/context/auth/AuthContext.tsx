import { createContext } from "react";
import { AuthContextType } from "../../interfaces/authInterfaces";

export const AuthContext = createContext<AuthContextType | undefined>(
  undefined
);
