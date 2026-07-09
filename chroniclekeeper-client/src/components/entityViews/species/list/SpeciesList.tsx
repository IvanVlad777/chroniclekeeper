import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { SpeciesDto } from "../../../../interfaces/loreInterfaces";
import { getSpecies } from "../../../../api/species";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function SpeciesList() {
    const navigate = useNavigate();
    const { t } = useTranslation("species");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [species, setSpecies] = useState<SpeciesDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setSpecies([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getSpecies(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setSpecies(data);
            })
            .catch((err) => {
                console.error("Failed to load species:", err);
                if (!cancelled) setError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, t, reloadKey]);

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="table" rows={6} />;
    }
    if (error) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⚘"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    const newButton = canCreate ? (
        <Button onClick={() => navigate("/storymap/species/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<SpeciesDto>[] = [
        {
            key: "name",
            header: t("columns.name"),
            sortable: true,
            render: (r) => (
                <span className={s.nameCell}>
                    <span className={s.glyph}>⚘</span>
                    <span className={s.name}>{r.name}</span>
                </span>
            ),
        },
        {
            key: "commonName",
            header: t("columns.commonName"),
            sortable: true,
            value: (r) => r.commonName ?? "",
            render: (r) => r.commonName || "—",
        },
        {
            key: "isHumanoid",
            header: t("columns.humanoid"),
            value: (r) => (r.isHumanoid ? "humanoid" : "other"),
            render: (r) => (
                <span className={s.muted}>
                    {r.isHumanoid ? t("humanoid") : t("notHumanoid")}
                </span>
            ),
        },
        {
            key: "lifespan",
            header: t("columns.lifespan"),
            value: (r) => r.lifespan ?? "",
            render: (r) => r.lifespan || "—",
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
            <DataTable<SpeciesDto>
                data={species}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) => navigate(`/storymap/species/${row.id}`)}
                title={t("listTitle")}
                initialSort={{ key: "name", dir: "asc" }}
                pageSize={10}
                searchPlaceholder={t("search")}
                action={newButton}
                empty={
                    <EmptyState
                        glyph="⚘"
                        title={t("emptyTitle")}
                        text={t("emptyText")}
                        action={newButton}
                    />
                }
            />
        </div>
    );
}
