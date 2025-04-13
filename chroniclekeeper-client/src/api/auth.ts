import axios from "axios";
import api from "../services/api";
import { AuthRequest } from "../interfaces/authInterfaces";

export const login = async (data: AuthRequest) => {
  try {
    const response = await api.post("/auth/login", data);
    return response.data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.error("Login error:", err.response?.data);
      throw err.response?.data || "Login failed";
    }
    console.error("Unexpected error during login:", err);
    throw "Unknown error during login";
  }
};

export const register = async (data: AuthRequest) => {
  try {
    const response = await api.post("/auth/register", data);
    return response.data;
  } catch (err) {
    if (axios.isAxiosError(err)) {
      console.error("Registration error:", err.response?.data);
      throw err.response?.data || "Registration failed";
    }
    console.error("Unexpected error during registration:", err);
    throw "Unknown error during registration";
  }
};
