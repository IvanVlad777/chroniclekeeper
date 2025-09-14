import { Routes, Route } from "react-router-dom";
import LoginPage from "../pages/LoginPage";
import RegisterPage from "../pages/RegisterPage";
import ProtectedRoute from "./ProtectedRoute";
import StoryMapPage from "../pages/StoryMapPage";

const AppRoutes = () => {
    return (
        <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route
                path="/storymap/*"
                element={
                    <ProtectedRoute>
                        <StoryMapPage />
                    </ProtectedRoute>
                }
            />
            <Route path="*" element={<LoginPage />} />
        </Routes>
    );
};

export default AppRoutes;
