import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReligiousTextDto } from "../../../../interfaces/loreInterfaces";
import { getReligiousTexts } from "../../../../api/religiousTexts";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "../mythology.module.css";

const glyph = "📜";

export default function ReligiousTextList() {
    const navigate = useNavigate();
    const { t } = useTranslation("mythology");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [items, setItems] = useState<ReligiousTextDto[]>([]);
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
        getReligiousTexts(selectedWorld.id)
            .then((data) => !cancelled && setItems(data))
            .catch(() => !cancelled && setError(t("religiousText.loadError")))
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, t, reloadKey]);

    if (worldLoading || loading) return <LoadingSkeleton variant="table" rows={6} />;
    if (error) return <ErrorState onRetry={refetch} detail={error} />;
    if (!selectedWorld)
        return (
            <EmptyState
                glyph={glyph}
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );

    const newButton = canCreate ? (
        <Button onClick={() => navigate("/storymap/religious-texts/new")}>
            + {t("religiousText.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<ReligiousTextDto>[] = [
        {
            key: "name",
            header: t("fields.name"),
            sortable: true,
            render: (r) => (
                <span className={s.nameCell}>
                    <span className={s.glyph}>{glyph}</span>
                    <span className={s.name}>{r.name}</span>
                </span>
            ),
        },
        { key: "type", header: t("religiousText.fields.type"), sortable: true, value: (r) => r.type },
        {
            key: "updatedAt",
            header: t("fields.updated"),
            sortable: true,
            align: "right",
            value: (r) => r.updatedAt ?? "",
            render: (r) => (
                <span className={s.updated}>
                    {r.updatedAt ? new Date(r.updatedAt).toLocaleDateString() : "—"}
                </span>
            ),
        },
    ];

    return (
        <div className={s.page}>
            <DataTable<ReligiousTextDto>
                data={items}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) => navigate(`/storymap/religious-texts/${row.id}`)}
                title={t("religiousText.listTitle")}
                initialSort={{ key: "name", dir: "asc" }}
                pageSize={10}
                searchPlaceholder={t("search")}
                action={newButton}
                empty={
                    <EmptyState glyph={glyph} title={t("religiousText.emptyTitle")} text={t("religiousText.emptyText")} action={newButton} />
                }
            />
        </div>
    );
}
