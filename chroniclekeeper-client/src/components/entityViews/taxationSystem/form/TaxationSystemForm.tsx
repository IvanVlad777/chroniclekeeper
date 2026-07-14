import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createTaxationSystem,
    deleteTaxationSystem,
    getTaxationSystemById,
    updateTaxationSystem,
} from "../../../../api/taxationSystems";
import { getHistories } from "../../../../api/histories";
import {
    HistoryDto,
    TaxationSystemUpdateDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    incomeTaxRate: string;
    corporateTaxRate: string;
    tradeTariffRate: string;
    hasFlatTax: boolean;
    hasWealthTax: boolean;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    incomeTaxRate: "0",
    corporateTaxRate: "0",
    tradeTariffRate: "0",
    hasFlatTax: false,
    hasWealthTax: false,
    historyId: "",
};

function toDto(f: FormState): TaxationSystemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        incomeTaxRate: Number(f.incomeTaxRate) || 0,
        corporateTaxRate: Number(f.corporateTaxRate) || 0,
        tradeTariffRate: Number(f.tradeTariffRate) || 0,
        hasFlatTax: f.hasFlatTax,
        hasWealthTax: f.hasWealthTax,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /taxation-systems/new i /taxation-systems/:id/edit. */
export default function TaxationSystemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("taxationSystem");
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
                    const ts = await getTaxationSystemById(editId);
                    if (cancelled) return;
                    setForm({
                        name: ts.name ?? "",
                        description: ts.description ?? "",
                        incomeTaxRate: String(ts.incomeTaxRate ?? 0),
                        corporateTaxRate: String(ts.corporateTaxRate ?? 0),
                        tradeTariffRate: String(ts.tradeTariffRate ?? 0),
                        hasFlatTax: ts.hasFlatTax,
                        hasWealthTax: ts.hasWealthTax,
                        historyId: ts.historyId ? String(ts.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load taxation system form data:", err);
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
                await updateTaxationSystem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createTaxationSystem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/taxation-systems/${targetId}`);
        } catch (err) {
            console.error("Failed to save taxation system:", err);
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
            await deleteTaxationSystem(editId);
            navigate("/storymap/taxation-systems");
        } catch (err) {
            console.error("Failed to delete taxation system:", err);
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
                glyph="⚖"
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

                <div className={s.col}>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.incomeTaxRate")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                max={100}
                                step="any"
                                value={form.incomeTaxRate}
                                onChange={(e) =>
                                    set("incomeTaxRate", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.corporateTaxRate")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                max={100}
                                step="any"
                                value={form.corporateTaxRate}
                                onChange={(e) =>
                                    set("corporateTaxRate", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.tradeTariffRate")}>
                        <OrnateTextInput
                            type="number"
                            min={0}
                            max={100}
                            step="any"
                            value={form.tradeTariffRate}
                            onChange={(e) =>
                                set("tradeTariffRate", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.hasFlatTax")}
                        checked={form.hasFlatTax}
                        onChange={(e) => set("hasFlatTax", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.hasWealthTax")}
                        checked={form.hasWealthTax}
                        onChange={(e) => set("hasWealthTax", e.target.checked)}
                    />
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
                                ? `/storymap/taxation-systems/${editId}`
                                : "/storymap/taxation-systems"
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
