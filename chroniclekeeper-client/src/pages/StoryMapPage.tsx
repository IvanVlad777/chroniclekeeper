// import { useState } from "react";
// import { useTranslation } from "react-i18next";

import { Route, Routes } from "react-router-dom";
import Overview from "../components/entityViews/overview/Overview";
import CharactersList from "../components/entityViews/character/list/CharacterList";

const StoryMapPage = () => {
    // const { t } = useTranslation("login");
    return (
        <div>
            <main style={{ padding: 24 }}>
                <Routes>
                    {/* index = Overview */}
                    <Route index element={<Overview />} />

                    {/* lista entiteta */}
                    <Route path="characters" element={<CharactersList />} />

                    {/* detalj entiteta */}
                    {/* <Route path="characters/:id" element={<CharacterDetail data={MOCK} />} /> */}

                    <Route path="*" element={<p>Not found</p>} />
                </Routes>
            </main>
        </div>
    );
};

export default StoryMapPage;
