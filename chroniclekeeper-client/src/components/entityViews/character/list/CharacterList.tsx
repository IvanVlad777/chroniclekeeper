import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable, StatusPill } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { CharacterDto } from "../../../../interfaces/loreInterfaces";
import { getCharacters } from "../../../../api/characters";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function CharactersList() {
    const navigate = useNavigate();
    const { t } = useTranslation("character");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [characters, setCharacters] = useState<CharacterDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setCharacters([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getCharacters(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setCharacters(data);
            })
            .catch((err) => {
                console.error("Failed to load characters:", err);
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
                glyph="♟"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    const columns: Column<CharacterDto>[] = [
        {
            key: "name",
            header: t("columns.name"),
            sortable: true,
            render: (r) => (
                <span className={s.nameCell}>
                    <span className={s.avatar}>{r.name?.charAt(0)}</span>
                    <span className={s.name}>{r.name}</span>
                </span>
            ),
        },
        {
            key: "title",
            header: t("columns.title"),
            sortable: true,
            value: (r) => r.title ?? "",
            render: (r) =>
                r.title ? <span className={s.title}>{r.title}</span> : "—",
        },
        {
            key: "status",
            header: t("columns.status"),
            value: (r) => (r.deathDate ? "dead" : "living"),
            render: (r) => (
                <StatusPill status={r.deathDate ? "dead" : "living"}>
                    {r.deathDate ? t("status.dead") : t("status.living")}
                </StatusPill>
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
            <DataTable<CharacterDto>
                data={characters}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) => navigate(`/storymap/characters/${row.id}`)}
                title={t("listTitle")}
                initialSort={{ key: "name", dir: "asc" }}
                pageSize={10}
                searchPlaceholder={t("search")}
                action={
                    canCreate ? (
                        <Button
                            onClick={() =>
                                navigate("/storymap/characters/new")
                            }
                        >
                            + {t("form.newTitle")}
                        </Button>
                    ) : undefined
                }
                empty={
                    <EmptyState
                        glyph="♟"
                        title={t("emptyTitle")}
                        text={t("emptyText")}
                        action={
                            canCreate ? (
                                <Button
                                    onClick={() =>
                                        navigate("/storymap/characters/new")
                                    }
                                >
                                    + {t("form.newTitle")}
                                </Button>
                            ) : undefined
                        }
                    />
                }
            />
        </div>
    );
}
