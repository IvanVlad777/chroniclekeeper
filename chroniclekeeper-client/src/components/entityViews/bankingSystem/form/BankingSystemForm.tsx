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
    createBankingSystem,
    deleteBankingSystem,
    getBankingSystemById,
    updateBankingSystem,
} from "../../../../api/bankingSystems";
import { getCurrencies } from "../../../../api/currencies";
import { getHistories } from "../../../../api/histories";
import {
    BankingSystemUpdateDto,
    CurrencyDto,
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
    systemType: string;
    interestRate: string;
    allowsLoans: boolean;
    hasStateControl: boolean;
    supportsForeignInvestment: boolean;
    currencyId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    systemType: "",
    interestRate: "0",
    allowsLoans: false,
    hasStateControl: false,
    supportsForeignInvestment: false,
    currencyId: "",
    historyId: "",
};

function toDto(f: FormState): BankingSystemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        systemType: f.systemType,
        interestRate: Number(f.interestRate) || 0,
        allowsLoans: f.allowsLoans,
        hasStateControl: f.hasStateControl,
        supportsForeignInvestment: f.supportsForeignInvestment,
        currencyId: f.currencyId ? Number(f.currencyId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /banking-systems/new i /banking-systems/:id/edit. */
export default function BankingSystemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("bankingSystem");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [currencies, setCurrencies] = useState<CurrencyDto[]>([]);
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

        Promise.all([
            getCurrencies(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(async ([currenciesData, historiesData]) => {
                if (cancelled) return;
                setCurrencies(currenciesData);
                setHistories(historiesData);

                if (isEdit) {
                    const b = await getBankingSystemById(editId);
                    if (cancelled) return;
                    setForm({
                        name: b.name ?? "",
                        description: b.description ?? "",
                        systemType: b.systemType ?? "",
                        interestRate: String(b.interestRate ?? 0),
                        allowsLoans: b.allowsLoans,
                        hasStateControl: b.hasStateControl,
                        supportsForeignInvestment: b.supportsForeignInvestment,
                        currencyId: b.currencyId ? String(b.currencyId) : "",
                        historyId: b.historyId ? String(b.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load banking system form data:", err);
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
                await updateBankingSystem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createBankingSystem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/banking-systems/${targetId}`);
        } catch (err) {
            console.error("Failed to save banking system:", err);
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
            await deleteBankingSystem(editId);
            navigate("/storymap/banking-systems");
        } catch (err) {
            console.error("Failed to delete banking system:", err);
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
                glyph="🏛"
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
                    <OrnateField label={t("fields.systemType")}>
                        <OrnateTextInput
                            value={form.systemType}
                            maxLength={100}
                            placeholder={t("fields.systemTypeHint")}
                            onChange={(e) => set("systemType", e.target.value)}
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.interestRate")}>
                            <OrnateTextInput
                                type="number"
                                step="any"
                                value={form.interestRate}
                                onChange={(e) =>
                                    set("interestRate", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.currency")}>
                            <OrnateSelect
                                value={form.currencyId}
                                onChange={(e) =>
                                    set("currencyId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {currencies.map((c) => (
                                    <option key={c.id} value={c.id}>
                                        {c.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("fields.allowsLoans")}
                        checked={form.allowsLoans}
                        onChange={(e) => set("allowsLoans", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.hasStateControl")}
                        checked={form.hasStateControl}
                        onChange={(e) =>
                            set("hasStateControl", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.supportsForeignInvestment")}
                        checked={form.supportsForeignInvestment}
                        onChange={(e) =>
                            set("supportsForeignInvestment", e.target.checked)
                        }
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
                                ? `/storymap/banking-systems/${editId}`
                                : "/storymap/banking-systems"
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
