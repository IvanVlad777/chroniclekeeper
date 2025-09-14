import { NavLink, Outlet } from "react-router-dom";
import { useTranslation } from "react-i18next";

const StoryMapPage = () => {
    const { t } = useTranslation();
    return (
        <div className="flex">
            <aside>
                <ol className="flex flex-col">
                    <NavLink to="." end>
                        {t("overview")}
                    </NavLink>
                    <NavLink to="characters">{t("character")}</NavLink>
                    {/* ect */}
                </ol>
            </aside>
            <main style={{ padding: 24 }}>
                <Outlet />
            </main>
        </div>
    );
};

export default StoryMapPage;
