import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createCurrency,
    deleteCurrency,
    getCurrencyById,
    updateCurrency,
} from "../../../../api/currencies";
import { getHistories } from "../../../../api/histories";
import {
    CurrencyUpdateDto,
    HistoryDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    symbol: string;
    exchangeRate: string;
    backingType: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    symbol: "",
    exchangeRate: "1",
    backingType: "",
    historyId: "",
};

function toDto(f: FormState): CurrencyUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        symbol: f.symbol,
        exchangeRate: Number(f.exchangeRate) || 0,
        backingType: f.backingType,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /currencies/new i /currencies/:id/edit. */
export default function CurrencyForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("currency");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
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
                setHistories(historiesData);

                if (isEdit) {
                    const c = await getCurrencyById(editId);
                    if (cancelled) return;
                    setForm({
                        name: c.name ?? "",
                        description: c.description ?? "",
                        symbol: c.symbol ?? "",
                        exchangeRate: String(c.exchangeRate ?? 0),
                        backingType: c.backingType ?? "",
                        historyId: c.historyId ? String(c.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load currency form data:", err);
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
                await updateCurrency(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createCurrency({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/currencies/${targetId}`);
        } catch (err) {
            console.error("Failed to save currency:", err);
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
            await deleteCurrency(editId);
            navigate("/storymap/currencies");
        } catch (err) {
            console.error("Failed to delete currency:", err);
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
                glyph="◉"
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
                    <div className={s.row2}>
                        <OrnateField label={t("fields.symbol")}>
                            <OrnateTextInput
                                value={form.symbol}
                                maxLength={10}
                                onChange={(e) => set("symbol", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.exchangeRate")}>
                            <OrnateTextInput
                                type="number"
                                step="any"
                                value={form.exchangeRate}
                                onChange={(e) =>
                                    set("exchangeRate", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.backingType")}>
                        <OrnateTextInput
                            value={form.backingType}
                            maxLength={100}
                            placeholder={t("fields.backingTypeHint")}
                            onChange={(e) => set("backingType", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("form.history")}>
                        <OrnateSelect
                            value={form.historyId}
                            onChange={(e) => set("historyId", e.target.value)}
                        >
                            <option value="">{t("none")}</option>
                            {histories.map((h) => (
                                <option key={h.id} value={h.id}>
                                    {h.name}
                                </option>
                            ))}
                        </OrnateSelect>
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
                                ? `/storymap/currencies/${editId}`
                                : "/storymap/currencies"
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
