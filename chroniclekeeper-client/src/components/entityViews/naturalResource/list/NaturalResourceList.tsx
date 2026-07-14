import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { NaturalResourceDto } from "../../../../interfaces/loreInterfaces";
import { getNaturalResources } from "../../../../api/naturalResources";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⛰";

export default function NaturalResourceList() {
    const navigate = useNavigate();
    const { t } = useTranslation("naturalResource");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [naturalResources, setNaturalResources] = useState<
        NaturalResourceDto[]
    >([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setNaturalResources([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getNaturalResources(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setNaturalResources(data);
            })
            .catch((err) => {
                console.error("Failed to load natural resources:", err);
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
        <Button onClick={() => navigate("/storymap/natural-resources/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<NaturalResourceDto>[] = [
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
            key: "resourceType",
            header: t("columns.resourceType"),
            sortable: true,
            value: (r) => r.resourceType,
        },
        {
            key: "marketValue",
            header: t("columns.marketValue"),
            sortable: true,
            align: "right",
            value: (r) => r.marketValue,
        },
        {
            key: "isStrategicResource",
            header: t("columns.isStrategicResource"),
            sortable: true,
            value: (r) =>
                r.isStrategicResource ? t("strategic.yes") : t("strategic.no"),
            render: (r) =>
                r.isStrategicResource ? (
                    <Tag tone="accent">{t("strategic.yes")}</Tag>
                ) : (
                    <span className={s.updated}>{t("strategic.no")}</span>
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
            <DataTable<NaturalResourceDto>
                data={naturalResources}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) =>
                    navigate(`/storymap/natural-resources/${row.id}`)
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
