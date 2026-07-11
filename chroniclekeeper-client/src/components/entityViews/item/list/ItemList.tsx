import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ItemDto } from "../../../../interfaces/loreInterfaces";
import { getItems } from "../../../../api/items";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⚔";

export default function ItemList() {
    const navigate = useNavigate();
    const { t } = useTranslation("item");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [items, setItems] = useState<ItemDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setItems([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getItems({ worldId: selectedWorld.id })
            .then((data) => {
                if (!cancelled) setItems(data);
            })
            .catch((err) => {
                console.error("Failed to load items:", err);
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
        <Button onClick={() => navigate("/storymap/items/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<ItemDto>[] = [
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
            key: "category",
            header: t("columns.category"),
            sortable: true,
            value: (r) => r.category ?? "",
            render: (r) => (
                <span className={s.meta}>
                    {r.category ? t(`categories.${r.category}`) : "—"}
                </span>
            ),
        },
        {
            key: "rarity",
            header: t("columns.rarity"),
            sortable: true,
            value: (r) => r.rarity ?? "",
            render: (r) => (
                <span className={s.meta}>
                    {r.rarity ? t(`rarities.${r.rarity}`) : "—"}
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
            <DataTable<ItemDto>
                data={items}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) => navigate(`/storymap/items/${row.id}`)}
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
