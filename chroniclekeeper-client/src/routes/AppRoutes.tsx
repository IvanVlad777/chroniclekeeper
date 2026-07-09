import { Routes, Route, Navigate } from "react-router-dom";
import AuthPage from "../pages/AuthPage";
import ProtectedRoute from "./ProtectedRoute";
import NoPage from "../pages/NoPage";
import Overview from "../components/entityViews/overview/Overview";
import CharactersList from "../components/entityViews/character/list/CharacterList";
import CharacterDetail from "../components/entityViews/character/detail/CharacterDetails";
import CharacterForm from "../components/entityViews/character/form/CharacterForm";
import LocationList from "../components/entityViews/location/list/LocationList";
import LocationDetails from "../components/entityViews/location/detail/LocationDetails";
import LocationForm from "../components/entityViews/location/form/LocationForm";
import FactionList from "../components/entityViews/faction/list/FactionList";
import FactionDetails from "../components/entityViews/faction/detail/FactionDetails";
import FactionForm from "../components/entityViews/faction/form/FactionForm";
import SpeciesList from "../components/entityViews/species/list/SpeciesList";
import SpeciesDetails from "../components/entityViews/species/detail/SpeciesDetails";
import SpeciesForm from "../components/entityViews/species/form/SpeciesForm";
import TimelineList from "../components/entityViews/timeline/list/TimelineList";
import TimelineDetails from "../components/entityViews/timeline/detail/TimelineDetails";
import TimelineForm from "../components/entityViews/timeline/form/TimelineForm";
import TagsPage from "../components/entityViews/tag/TagsPage";
import NotesPage from "../components/entityViews/note/NotesPage";
import { WorldProvider } from "../context/world/WorldProvider";
import { AppShell } from "../components/shell/AppShell";

const AppRoutes = () => {
    return (
        <Routes>
            <Route index element={<Navigate to="/storymap" replace />} />
            <Route
                path="/login"
                element={<AuthPage initialMode="signin" />}
            />
            <Route
                path="/register"
                element={<AuthPage initialMode="register" />}
            />
            <Route
                path="/storymap"
                element={
                    <ProtectedRoute>
                        <WorldProvider>
                            <AppShell />
                        </WorldProvider>
                    </ProtectedRoute>
                }
            >
                <Route index element={<Overview />} />
                <Route path="overview" element={<Overview />} />
                <Route path="characters" element={<CharactersList />} />
                <Route path="characters/new" element={<CharacterForm />} />
                <Route path="characters/:id" element={<CharacterDetail />} />
                <Route
                    path="characters/:id/edit"
                    element={<CharacterForm />}
                />
                <Route path="locations" element={<LocationList />} />
                <Route path="locations/new" element={<LocationForm />} />
                <Route path="locations/:id" element={<LocationDetails />} />
                <Route
                    path="locations/:id/edit"
                    element={<LocationForm />}
                />
                <Route path="factions" element={<FactionList />} />
                <Route path="factions/new" element={<FactionForm />} />
                <Route path="factions/:id" element={<FactionDetails />} />
                <Route
                    path="factions/:id/edit"
                    element={<FactionForm />}
                />
                <Route path="species" element={<SpeciesList />} />
                <Route path="species/new" element={<SpeciesForm />} />
                <Route path="species/:id" element={<SpeciesDetails />} />
                <Route
                    path="species/:id/edit"
                    element={<SpeciesForm />}
                />
                <Route path="timelines" element={<TimelineList />} />
                <Route path="timelines/new" element={<TimelineForm />} />
                <Route path="timelines/:id" element={<TimelineDetails />} />
                <Route
                    path="timelines/:id/edit"
                    element={<TimelineForm />}
                />
                <Route path="tags" element={<TagsPage />} />
                <Route path="notes" element={<NotesPage />} />
                <Route path="*" element={<NoPage />} />
            </Route>

            <Route path="*" element={<NoPage />} />
        </Routes>
    );
};

export default AppRoutes;
