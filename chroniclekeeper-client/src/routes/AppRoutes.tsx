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
import SocialClassList from "../components/entityViews/socialClass/list/SocialClassList";
import SocialClassDetails from "../components/entityViews/socialClass/detail/SocialClassDetails";
import SocialClassForm from "../components/entityViews/socialClass/form/SocialClassForm";
import NationList from "../components/entityViews/nation/list/NationList";
import NationDetails from "../components/entityViews/nation/detail/NationDetails";
import NationForm from "../components/entityViews/nation/form/NationForm";
import ReligionList from "../components/entityViews/religion/list/ReligionList";
import ReligionDetails from "../components/entityViews/religion/detail/ReligionDetails";
import ReligionForm from "../components/entityViews/religion/form/ReligionForm";
import LanguageList from "../components/entityViews/language/list/LanguageList";
import LanguageDetails from "../components/entityViews/language/detail/LanguageDetails";
import LanguageForm from "../components/entityViews/language/form/LanguageForm";
import CultureList from "../components/entityViews/culture/list/CultureList";
import CultureDetails from "../components/entityViews/culture/detail/CultureDetails";
import CultureForm from "../components/entityViews/culture/form/CultureForm";
import TimelineList from "../components/entityViews/timeline/list/TimelineList";
import TimelineDetails from "../components/entityViews/timeline/detail/TimelineDetails";
import TimelineForm from "../components/entityViews/timeline/form/TimelineForm";
import TagsPage from "../components/entityViews/tag/TagsPage";
import NotesPage from "../components/entityViews/note/NotesPage";
import PoliticalIdeologyList from "../components/entityViews/politicalIdeology/list/PoliticalIdeologyList";
import PoliticalIdeologyDetails from "../components/entityViews/politicalIdeology/detail/PoliticalIdeologyDetails";
import PoliticalIdeologyForm from "../components/entityViews/politicalIdeology/form/PoliticalIdeologyForm";
import GovernmentSystemList from "../components/entityViews/governmentSystem/list/GovernmentSystemList";
import GovernmentSystemDetails from "../components/entityViews/governmentSystem/detail/GovernmentSystemDetails";
import GovernmentSystemForm from "../components/entityViews/governmentSystem/form/GovernmentSystemForm";
import PoliticalPartyList from "../components/entityViews/politicalParty/list/PoliticalPartyList";
import PoliticalPartyDetails from "../components/entityViews/politicalParty/detail/PoliticalPartyDetails";
import PoliticalPartyForm from "../components/entityViews/politicalParty/form/PoliticalPartyForm";
import LegalSystemList from "../components/entityViews/legalSystem/list/LegalSystemList";
import LegalSystemDetails from "../components/entityViews/legalSystem/detail/LegalSystemDetails";
import LegalSystemForm from "../components/entityViews/legalSystem/form/LegalSystemForm";
import DiplomaticAgreementList from "../components/entityViews/diplomaticAgreement/list/DiplomaticAgreementList";
import DiplomaticAgreementDetails from "../components/entityViews/diplomaticAgreement/detail/DiplomaticAgreementDetails";
import DiplomaticAgreementForm from "../components/entityViews/diplomaticAgreement/form/DiplomaticAgreementForm";
import ProfessionList from "../components/entityViews/profession/list/ProfessionList";
import ProfessionDetails from "../components/entityViews/profession/detail/ProfessionDetails";
import ProfessionForm from "../components/entityViews/profession/form/ProfessionForm";
import SchoolList from "../components/entityViews/school/list/SchoolList";
import SchoolDetails from "../components/entityViews/school/detail/SchoolDetails";
import SchoolForm from "../components/entityViews/school/form/SchoolForm";
import TradeSchoolForm from "../components/entityViews/school/form/TradeSchoolForm";
import UniversityList from "../components/entityViews/university/list/UniversityList";
import UniversityDetails from "../components/entityViews/university/detail/UniversityDetails";
import UniversityForm from "../components/entityViews/university/form/UniversityForm";
import EducationSystemList from "../components/entityViews/educationSystem/list/EducationSystemList";
import EducationSystemDetails from "../components/entityViews/educationSystem/detail/EducationSystemDetails";
import EducationSystemForm from "../components/entityViews/educationSystem/form/EducationSystemForm";
import LibraryList from "../components/entityViews/library/list/LibraryList";
import LibraryDetails from "../components/entityViews/library/detail/LibraryDetails";
import LibraryForm from "../components/entityViews/library/form/LibraryForm";
import AbilityList from "../components/entityViews/ability/list/AbilityList";
import AbilityDetails from "../components/entityViews/ability/detail/AbilityDetails";
import AbilityForm from "../components/entityViews/ability/form/AbilityForm";
import ItemList from "../components/entityViews/item/list/ItemList";
import ItemDetails from "../components/entityViews/item/detail/ItemDetails";
import ItemForm from "../components/entityViews/item/form/ItemForm";
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
                <Route
                    path="social-classes"
                    element={<SocialClassList />}
                />
                <Route
                    path="social-classes/new"
                    element={<SocialClassForm />}
                />
                <Route
                    path="social-classes/:id"
                    element={<SocialClassDetails />}
                />
                <Route
                    path="social-classes/:id/edit"
                    element={<SocialClassForm />}
                />
                <Route path="nations" element={<NationList />} />
                <Route path="nations/new" element={<NationForm />} />
                <Route path="nations/:id" element={<NationDetails />} />
                <Route
                    path="nations/:id/edit"
                    element={<NationForm />}
                />
                <Route path="religions" element={<ReligionList />} />
                <Route path="religions/new" element={<ReligionForm />} />
                <Route path="religions/:id" element={<ReligionDetails />} />
                <Route
                    path="religions/:id/edit"
                    element={<ReligionForm />}
                />
                <Route path="languages" element={<LanguageList />} />
                <Route path="languages/new" element={<LanguageForm />} />
                <Route path="languages/:id" element={<LanguageDetails />} />
                <Route
                    path="languages/:id/edit"
                    element={<LanguageForm />}
                />
                <Route path="cultures" element={<CultureList />} />
                <Route path="cultures/new" element={<CultureForm />} />
                <Route path="cultures/:id" element={<CultureDetails />} />
                <Route
                    path="cultures/:id/edit"
                    element={<CultureForm />}
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
                <Route
                    path="political-ideologies"
                    element={<PoliticalIdeologyList />}
                />
                <Route
                    path="political-ideologies/new"
                    element={<PoliticalIdeologyForm />}
                />
                <Route
                    path="political-ideologies/:id"
                    element={<PoliticalIdeologyDetails />}
                />
                <Route
                    path="political-ideologies/:id/edit"
                    element={<PoliticalIdeologyForm />}
                />
                <Route
                    path="government-systems"
                    element={<GovernmentSystemList />}
                />
                <Route
                    path="government-systems/new"
                    element={<GovernmentSystemForm />}
                />
                <Route
                    path="government-systems/:id"
                    element={<GovernmentSystemDetails />}
                />
                <Route
                    path="government-systems/:id/edit"
                    element={<GovernmentSystemForm />}
                />
                <Route
                    path="political-parties"
                    element={<PoliticalPartyList />}
                />
                <Route
                    path="political-parties/new"
                    element={<PoliticalPartyForm />}
                />
                <Route
                    path="political-parties/:id"
                    element={<PoliticalPartyDetails />}
                />
                <Route
                    path="political-parties/:id/edit"
                    element={<PoliticalPartyForm />}
                />
                <Route path="legal-systems" element={<LegalSystemList />} />
                <Route
                    path="legal-systems/new"
                    element={<LegalSystemForm />}
                />
                <Route
                    path="legal-systems/:id"
                    element={<LegalSystemDetails />}
                />
                <Route
                    path="legal-systems/:id/edit"
                    element={<LegalSystemForm />}
                />
                <Route
                    path="diplomatic-agreements"
                    element={<DiplomaticAgreementList />}
                />
                <Route
                    path="diplomatic-agreements/new"
                    element={<DiplomaticAgreementForm />}
                />
                <Route
                    path="diplomatic-agreements/:id"
                    element={<DiplomaticAgreementDetails />}
                />
                <Route
                    path="diplomatic-agreements/:id/edit"
                    element={<DiplomaticAgreementForm />}
                />
                <Route path="professions" element={<ProfessionList />} />
                <Route path="professions/new" element={<ProfessionForm />} />
                <Route
                    path="professions/:id"
                    element={<ProfessionDetails />}
                />
                <Route
                    path="professions/:id/edit"
                    element={<ProfessionForm />}
                />
                <Route path="schools" element={<SchoolList />} />
                <Route path="schools/new" element={<SchoolForm />} />
                <Route
                    path="schools/new-trade"
                    element={<TradeSchoolForm />}
                />
                <Route path="schools/:id" element={<SchoolDetails />} />
                <Route path="schools/:id/edit" element={<SchoolForm />} />
                <Route
                    path="schools/:id/edit-trade"
                    element={<TradeSchoolForm />}
                />
                <Route path="universities" element={<UniversityList />} />
                <Route
                    path="universities/new"
                    element={<UniversityForm />}
                />
                <Route
                    path="universities/:id"
                    element={<UniversityDetails />}
                />
                <Route
                    path="universities/:id/edit"
                    element={<UniversityForm />}
                />
                <Route
                    path="education-systems"
                    element={<EducationSystemList />}
                />
                <Route
                    path="education-systems/new"
                    element={<EducationSystemForm />}
                />
                <Route
                    path="education-systems/:id"
                    element={<EducationSystemDetails />}
                />
                <Route
                    path="education-systems/:id/edit"
                    element={<EducationSystemForm />}
                />
                <Route path="libraries" element={<LibraryList />} />
                <Route path="libraries/new" element={<LibraryForm />} />
                <Route path="libraries/:id" element={<LibraryDetails />} />
                <Route
                    path="libraries/:id/edit"
                    element={<LibraryForm />}
                />
                <Route path="abilities" element={<AbilityList />} />
                <Route path="abilities/new" element={<AbilityForm />} />
                <Route path="abilities/:id" element={<AbilityDetails />} />
                <Route
                    path="abilities/:id/edit"
                    element={<AbilityForm />}
                />
                <Route path="items" element={<ItemList />} />
                <Route path="items/new" element={<ItemForm />} />
                <Route path="items/:id" element={<ItemDetails />} />
                <Route path="items/:id/edit" element={<ItemForm />} />
                <Route path="*" element={<NoPage />} />
            </Route>

            <Route path="*" element={<NoPage />} />
        </Routes>
    );
};

export default AppRoutes;
