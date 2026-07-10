import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { GovernmentSystemDto } from "../../../../interfaces/loreInterfaces";
import { getGovernmentSystems } from "../../../../api/governmentSystems";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⌂";

export default function GovernmentSystemList() {
    const navigate = useNavigate();
    const { t } = useTranslation("governmentSystem");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [systems, setSystems] = useState<GovernmentSystemDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setSystems([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getGovernmentSystems(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setSystems(data);
            })
            .catch((err) => {
                console.error("Failed to load government systems:", err);
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
                glyph={glyph}
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    const newButton = canCreate ? (
        <Button onClick={() => navigate("/storymap/government-systems/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<GovernmentSystemDto>[] = [
        {
            key: "name",
            header: t("columns.name"),
            sortable: true,
            render: (r) => (
                <span className={s.nameCell}>
                    <span className={s.glyph}>{glyph}</span>
                    <span className={s.name}>{r.name}</span>
                </span>
            ),
        },
        {
            key: "electionSystem",
            header: t("columns.electionSystem"),
            sortable: true,
            value: (r) => t(`electionSystems.${r.electionSystem}`),
            render: (r) => (
                <span className={s.muted}>
                    {t(`electionSystems.${r.electionSystem}`)}
                </span>
            ),
        },
        {
            key: "stabilityLevel",
            header: t("columns.stabilityLevel"),
            sortable: true,
            value: (r) => t(`scaleLevels.${r.stabilityLevel}`),
            render: (r) => (
                <span className={s.muted}>
                    {t(`scaleLevels.${r.stabilityLevel}`)}
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
            <DataTable<GovernmentSystemDto>
                data={systems}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) =>
                    navigate(`/storymap/government-systems/${row.id}`)
                }
                title={t("listTitle")}
                initialSort={{ key: "name", dir: "asc" }}
                pageSize={10}
                searchPlaceholder={t("search")}
                action={newButton}
                empty={
                    <EmptyState
                        glyph={glyph}
                        title={t("emptyTitle")}
                        text={t("emptyText")}
                        action={newButton}
                    />
                }
            />
        </div>
    );
}
