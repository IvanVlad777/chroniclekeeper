import { Routes, Route } from "react-router-dom";
import LoginPage from "../pages/LoginPage";
import RegisterPage from "../pages/RegisterPage";
import ProtectedRoute from "./ProtectedRoute";
import StoryMapPage from "../pages/StoryMapPage";
import NoPage from "../pages/NoPage";
import Overview from "../components/entityViews/overview/Overview";
import CharactersList from "../components/entityViews/character/list/CharacterList";
import CharacterDetail from "../components/entityViews/character/detail/CharacterDetails";

const AppRoutes = () => {
    return (
        <Routes>
            <Route index element={<Overview />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route
                path="/storymap"
                element={
                    <ProtectedRoute>
                        <StoryMapPage />
                    </ProtectedRoute>
                }
            >
                <Route index element={<Overview />} />
                <Route path="overview" element={<Overview />} />
                <Route path="characters" element={<CharactersList />} />
                <Route path="characters/:id" element={<CharacterDetail />} />
                <Route path="*" element={<NoPage />} />
            </Route>

            <Route path="*" element={<NoPage />} />
        </Routes>
    );
};

export default AppRoutes;
