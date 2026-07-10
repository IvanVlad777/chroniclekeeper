import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { DiplomaticAgreementDto } from "../../../../interfaces/loreInterfaces";
import { getDiplomaticAgreements } from "../../../../api/diplomaticAgreements";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⚜";

export default function DiplomaticAgreementList() {
    const navigate = useNavigate();
    const { t } = useTranslation("diplomaticAgreement");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [agreements, setAgreements] = useState<DiplomaticAgreementDto[]>(
        []
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setAgreements([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getDiplomaticAgreements(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setAgreements(data);
            })
            .catch((err) => {
                console.error("Failed to load diplomatic agreements:", err);
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
        <Button
            onClick={() => navigate("/storymap/diplomatic-agreements/new")}
        >
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const columns: Column<DiplomaticAgreementDto>[] = [
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
            key: "agreementType",
            header: t("columns.agreementType"),
            sortable: true,
            value: (r) => r.agreementType,
            render: (r) => (
                <span className={s.muted}>{r.agreementType || "—"}</span>
            ),
        },
        {
            key: "signedDate",
            header: t("columns.signedDate"),
            sortable: true,
            value: (r) => r.signedDate,
            render: (r) => (
                <span className={s.muted}>{r.signedDate || "—"}</span>
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
            <DataTable<DiplomaticAgreementDto>
                data={agreements}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) =>
                    navigate(`/storymap/diplomatic-agreements/${row.id}`)
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
