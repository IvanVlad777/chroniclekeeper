import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, Column, DataTable, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { SocialClassDto } from "../../../../interfaces/loreInterfaces";
import { getSocialClasses } from "../../../../api/socialClasses";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

const glyph = "⚖";

export default function SocialClassList() {
    const navigate = useNavigate();
    const { t } = useTranslation("socialClass");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [socialClasses, setSocialClasses] = useState<SocialClassDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setSocialClasses([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getSocialClasses(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setSocialClasses(data);
            })
            .catch((err) => {
                console.error("Failed to load social classes:", err);
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
        <Button onClick={() => navigate("/storymap/social-classes/new")}>
            + {t("form.newTitle")}
        </Button>
    ) : undefined;

    const traitList = (r: SocialClassDto) =>
        [
            r.isNoble && t("fields.isNoble"),
            r.isMerchantClass && t("fields.isMerchantClass"),
            r.isOutcast && t("fields.isOutcast"),
            r.canOwnLand && t("fields.canOwnLand"),
            r.canHoldOffice && t("fields.canHoldOffice"),
            r.hasTaxExemptions && t("fields.hasTaxExemptions"),
        ].filter((v): v is string => Boolean(v));

    const columns: Column<SocialClassDto>[] = [
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
            key: "traits",
            header: t("columns.traits"),
            value: (r) => traitList(r).join(" "),
            render: (r) => {
                const traits = traitList(r);
                return traits.length ? (
                    <span className={s.traits}>
                        {traits.map((tr) => (
                            <Tag key={tr} tone="neutral">
                                {tr}
                            </Tag>
                        ))}
                    </span>
                ) : (
                    <span className={s.muted}>—</span>
                );
            },
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
            <DataTable<SocialClassDto>
                data={socialClasses}
                columns={columns}
                getRowId={(r) => String(r.id)}
                onRowClick={(row) =>
                    navigate(`/storymap/social-classes/${row.id}`)
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
