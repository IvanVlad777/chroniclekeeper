import axios from "axios";

const api = axios.create({
  baseURL: "/api",
});

//Ovo bi mi trebalo osigurati da se automatski šalje token ako postoji u session storage
api.interceptors.request.use((config) => {
  const token = sessionStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
