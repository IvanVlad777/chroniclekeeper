import { NavLink, Outlet } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { useWorld } from "../hooks/useWorld";

const StoryMapPage = () => {
    const { t } = useTranslation();
    const { worlds, selectedWorld, selectWorld, loading, error } = useWorld();

    return (
        <div className="flex">
            <aside>
                <div style={{ marginBottom: 16 }}>
                    <label htmlFor="world-select" style={{ display: "block" }}>
                        {t("world") || "World"}
                    </label>
                    {loading ? (
                        <span>…</span>
                    ) : error ? (
                        <span>{error}</span>
                    ) : worlds.length === 0 ? (
                        <span>{t("noworlds") || "No worlds yet"}</span>
                    ) : (
                        <select
                            id="world-select"
                            value={selectedWorld?.id ?? ""}
                            onChange={(e) => selectWorld(Number(e.target.value))}
                        >
                            {worlds.map((w) => (
                                <option key={w.id} value={w.id}>
                                    {w.name}
                                </option>
                            ))}
                        </select>
                    )}
                </div>
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
