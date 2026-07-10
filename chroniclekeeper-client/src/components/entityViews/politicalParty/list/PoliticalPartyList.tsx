import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { PoliticalPartyDto } from "../../../../interfaces/loreInterfaces";
import { getPoliticalParties } from "../../../../api/politicalParties";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "✪";

export default function PoliticalPartyList() {
    const navigate = useNavigate();
    const { t } = useTranslation("politicalParty");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [parties, setParties] = useState<PoliticalPartyDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setParties([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getPoliticalParties(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setParties(data);
            })
            .catch((err) => {
                console.error("Failed to load political parties:", err);
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
        <Button onClick={() => navigate("/storymap/political-parties/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<PoliticalPartyDto>[] = [
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
            key: "influenceLevel",
            header: t("columns.influenceLevel"),
            sortable: true,
            value: (r) => t(`scaleLevels.${r.influenceLevel}`),
            render: (r) => (
                <span className={s.muted}>
                    {t(`scaleLevels.${r.influenceLevel}`)}
                </span>
            ),
        },
        {
            key: "isBanned",
            header: t("columns.isBanned"),
            value: (r) => (r.isBanned ? "1" : "0"),
            render: (r) =>
                r.isBanned ? (
                    <Tag tone="accent">{t("fields.isBanned")}</Tag>
                ) : (
                    <span className={s.muted}>—</span>
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
            <DataTable<PoliticalPartyDto>
                data={parties}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) =>
                    navigate(`/storymap/political-parties/${row.id}`)
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
