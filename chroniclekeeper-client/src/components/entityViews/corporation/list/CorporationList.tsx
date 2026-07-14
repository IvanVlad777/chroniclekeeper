import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { CorporationDto } from "../../../../interfaces/loreInterfaces";
import { getCorporations } from "../../../../api/corporations";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "🏢";

export default function CorporationList() {
    const navigate = useNavigate();
    const { t } = useTranslation("corporation");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [corporations, setCorporations] = useState<CorporationDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setCorporations([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getCorporations(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setCorporations(data);
            })
            .catch((err) => {
                console.error("Failed to load corporations:", err);
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
        <Button onClick={() => navigate("/storymap/corporations/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<CorporationDto>[] = [
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
            key: "industrySector",
            header: t("columns.industrySector"),
            sortable: true,
            value: (r) => r.industrySector,
        },
        {
            key: "numberOfEmployees",
            header: t("columns.numberOfEmployees"),
            sortable: true,
            align: "right",
            value: (r) => r.numberOfEmployees,
        },
        {
            key: "ownership",
            header: t("columns.ownership"),
            sortable: true,
            value: (r) =>
                r.isStateOwned
                    ? t("ownership.state")
                    : r.isPubliclyTraded
                      ? t("ownership.public")
                      : t("ownership.private"),
            render: (r) => (
                <Tag tone="neutral">
                    {r.isStateOwned
                        ? t("ownership.state")
                        : r.isPubliclyTraded
                          ? t("ownership.public")
                          : t("ownership.private")}
                </Tag>
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
            <DataTable<CorporationDto>
                data={corporations}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) => navigate(`/storymap/corporations/${row.id}`)}
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
