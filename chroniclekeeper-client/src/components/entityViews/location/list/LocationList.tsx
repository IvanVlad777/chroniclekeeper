import { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LocationDto, LocationType } from "../../../../interfaces/loreInterfaces";
import { getLocations } from "../../../../api/locations";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { locationGlyphs } from "../locationGlyphs";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function LocationList() {
    const navigate = useNavigate();
    const { t } = useTranslation("location");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [locations, setLocations] = useState<LocationDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [typeFilter, setTypeFilter] = useState<LocationType | "All">("All");
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setLocations([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getLocations(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setLocations(data);
            })
            .catch((err) => {
                console.error("Failed to load locations:", err);
                if (!cancelled) setError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, t, reloadKey]);

    // id → ime za kolonu "Parent" (bez dodatnih poziva)
    const nameById = useMemo(
        () => new Map(locations.map((l) => [l.id, l.name])),
        [locations]
    );
    const presentTypes = useMemo(
        () =>
            [...new Set(locations.map((l) => l.type))].sort((a, b) =>
                a.localeCompare(b)
            ),
        [locations]
    );
    const filtered = useMemo(
        () =>
            typeFilter === "All"
                ? locations
                : locations.filter((l) => l.type === typeFilter),
        [locations, typeFilter]
    );

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="table" rows={6} />;
    }
    if (error) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⚑"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    const newButton = canCreate ? (
        <Button onClick={() => navigate("/storymap/locations/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<LocationDto>[] = [
        {
            key: "name",
            header: t("columns.name"),
            sortable: true,
            render: (r) => (
                <span className={s.nameCell}>
                    <span className={s.glyph}>{locationGlyphs[r.type]}</span>
                    <span className={s.name}>{r.name}</span>
                </span>
            ),
        },
        {
            key: "type",
            header: t("columns.type"),
            sortable: true,
            value: (r) => r.type,
            render: (r) => (
                <span className={s.type}>{t(`types.${r.type}`)}</span>
            ),
        },
        {
            key: "parent",
            header: t("columns.parent"),
            sortable: true,
            value: (r) =>
                r.parentLocationId
                    ? nameById.get(r.parentLocationId) ?? ""
                    : "",
            render: (r) =>
                r.parentLocationId
                    ? nameById.get(r.parentLocationId) ?? "—"
                    : "—",
        },
        {
            key: "population",
            header: t("columns.population"),
            sortable: true,
            value: (r) => r.population ?? -1,
            render: (r) => (
                <span className={s.population}>
                    {r.population != null
                        ? r.population.toLocaleString()
                        : "—"}
                </span>
            ),
        },
        {
            key: "updatedAt",
            header: t("columns.updated"),
            sortable: true,
            align: "right",
            value: (r) => r.updatedAt ?? "",
            render: (r) => (
                <span className={s.updated}>
                    {r.updatedAt
                        ? new Date(r.updatedAt).toLocaleDateString()
                        : "—"}
                </span>
            ),
        },
    ];

    return (
        <div className={s.page}>
            {presentTypes.length > 1 && (
                <div className={s.filters}>
                    <button
                        type="button"
                        className={`${s.filterChip} ${
                            typeFilter === "All" ? s.filterChipActive : ""
                        }`}
                        onClick={() => setTypeFilter("All")}
                    >
                        {t("filterAll")}
                    </button>
                    {presentTypes.map((type) => (
                        <button
                            key={type}
                            type="button"
                            className={`${s.filterChip} ${
                                typeFilter === type ? s.filterChipActive : ""
                            }`}
                            onClick={() => setTypeFilter(type)}
                        >
                            {t(`types.${type}`)}
                        </button>
                    ))}
                </div>
            )}
            <DataTable<LocationDto>
                data={filtered}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) => navigate(`/storymap/locations/${row.id}`)}
                title={t("listTitle")}
                initialSort={{ key: "name", dir: "asc" }}
                pageSize={10}
                searchPlaceholder={t("search")}
                action={newButton}
                empty={
                    <EmptyState
                        glyph="⚑"
                        title={t("emptyTitle")}
                        text={t("emptyText")}
                        action={newButton}
                    />
                }
            />
        </div>
    );
}
