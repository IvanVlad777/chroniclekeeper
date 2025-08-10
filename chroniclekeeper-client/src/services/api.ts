import axios from "axios";

const api = axios.create({
    baseURL: "/api",
});

//Ovo bi mi trebalo osigurati da se automatski šalje token ako postoji u session storage
api.interceptors.request.use((config) => {
    const token = sessionStorage.getItem("token");
    if (token) {
        config.headers = config.headers ?? {};
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

api.interceptors.response.use(
    (res) => res,
    (err) => {
        const status = err?.response?.status;
        if (status === 401 || status === 403) {
            sessionStorage.removeItem("token");
            sessionStorage.removeItem("user");
            // window.location.assign("/login"); // po potrebi
        }
        return Promise.reject(err);
    }
);

export default api;
