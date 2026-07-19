import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    EntityPicker,
    type EntityOption,
} from "../../../quickCreate/EntityPicker";
import {
    createTradeRoute,
    deleteTradeRoute,
    getTradeRouteById,
    updateTradeRoute,
} from "../../../../api/tradeRoutes";
import { getHistories } from "../../../../api/histories";
import {
    TradeRouteUpdateDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    routeType: string;
    mainGoods: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    routeType: "",
    mainGoods: "",
    historyId: "",
};

function toDto(f: FormState): TradeRouteUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        routeType: f.routeType,
        mainGoods: f.mainGoods,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /trade-routes/new i /trade-routes/:id/edit. */
export default function TradeRouteForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("tradeRoute");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [historyOptions, setHistoryOptions] = useState<EntityOption[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getHistories(selectedWorld.id)
            .then(async (historiesData) => {
                if (cancelled) return;
                setHistoryOptions(
                    historiesData.map((h) => ({ value: h.id, label: h.name }))
                );

                if (isEdit) {
                    const tr = await getTradeRouteById(editId);
                    if (cancelled) return;
                    setForm({
                        name: tr.name ?? "",
                        description: tr.description ?? "",
                        routeType: tr.routeType ?? "",
                        mainGoods: tr.mainGoods ?? "",
                        historyId: tr.historyId ? String(tr.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load trade route form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateTradeRoute(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createTradeRoute({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/trade-routes/${targetId}`);
        } catch (err) {
            console.error("Failed to save trade route:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) {
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            await deleteTradeRoute(editId);
            navigate("/storymap/trade-routes");
        } catch (err) {
            console.error("Failed to delete trade route:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⛵"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }
    if (loadError) {
        return (
            <ErrorState
                onRetry={() => setReloadKey((k) => k + 1)}
                detail={loadError}
            />
        );
    }

    return (
        <form className={s.page} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>
                {isEdit ? t("form.editTitle") : t("form.newTitle")}
            </h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("columns.name")} required>
                        <OrnateTextInput
                            value={form.name}
                            display
                            maxLength={100}
                            onChange={(e) => set("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={6}
                            maxLength={4000}
                            onChange={(e) => set("description", e.target.value)}
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.routeType")}>
                        <OrnateTextInput
                            value={form.routeType}
                            maxLength={50}
                            placeholder={t("fields.routeTypeHint")}
                            onChange={(e) => set("routeType", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.mainGoods")}>
                        <OrnateTextInput
                            value={form.mainGoods}
                            maxLength={200}
                            placeholder={t("fields.mainGoodsHint")}
                            onChange={(e) => set("mainGoods", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("form.history")}>
                        <EntityPicker
                            kind="history"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.historyId}
                            options={historyOptions}
                            onChange={(v) => set("historyId", v)}
                            onCreated={(h) =>
                                setHistoryOptions((prev) => [
                                    ...prev,
                                    { value: h.id, label: h.name },
                                ])
                            }
                        />
                    </OrnateField>
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button variant="danger" disabled={busy} onClick={onDelete}>
                        {t("form.delete")}
                    </Button>
                )}
                <span className={s.footerSpacer} />
                <Button
                    variant="ghost"
                    disabled={busy}
                    onClick={() =>
                        navigate(
                            isEdit
                                ? `/storymap/trade-routes/${editId}`
                                : "/storymap/trade-routes"
                        )
                    }
                >
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>
                    {busy ? t("form.saving") : t("form.save")}
                </Button>
            </div>
        </form>
    );
}
