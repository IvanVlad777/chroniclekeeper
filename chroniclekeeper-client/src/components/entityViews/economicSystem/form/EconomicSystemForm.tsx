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
    createEconomicSystem,
    deleteEconomicSystem,
    getEconomicSystemById,
    updateEconomicSystem,
} from "../../../../api/economicSystems";
import { getBankingSystems } from "../../../../api/bankingSystems";
import { getTaxationSystems } from "../../../../api/taxationSystems";
import { getHistories } from "../../../../api/histories";
import {
    BankingSystemDto,
    EconomicSystemUpdateDto,
    HistoryDto,
    TaxationSystemDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isMarketDriven: boolean;
    hasStateControl: boolean;
    isFeudal: boolean;
    allowsCorporations: boolean;
    allowsGuilds: boolean;
    taxationSystemId: string;
    bankingSystemId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isMarketDriven: true,
    hasStateControl: false,
    isFeudal: false,
    allowsCorporations: true,
    allowsGuilds: true,
    taxationSystemId: "",
    bankingSystemId: "",
    historyId: "",
};

function toDto(f: FormState): EconomicSystemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isMarketDriven: f.isMarketDriven,
        hasStateControl: f.hasStateControl,
        isFeudal: f.isFeudal,
        allowsCorporations: f.allowsCorporations,
        allowsGuilds: f.allowsGuilds,
        taxationSystemId: f.taxationSystemId ? Number(f.taxationSystemId) : null,
        bankingSystemId: f.bankingSystemId ? Number(f.bankingSystemId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /economic-systems/new i /economic-systems/:id/edit. */
export default function EconomicSystemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("economicSystem");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [taxationSystems, setTaxationSystems] = useState<TaxationSystemDto[]>(
        []
    );
    const [bankingSystems, setBankingSystems] = useState<BankingSystemDto[]>([]);
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
            getTaxationSystems(selectedWorld.id),
            getBankingSystems(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(async ([taxationData, bankingData, historiesData]) => {
                if (cancelled) return;
                setTaxationSystems(taxationData);
                setBankingSystems(bankingData);
                setHistories(historiesData);

                if (isEdit) {
                    const es = await getEconomicSystemById(editId);
                    if (cancelled) return;
                    setForm({
                        name: es.name ?? "",
                        description: es.description ?? "",
                        isMarketDriven: es.isMarketDriven,
                        hasStateControl: es.hasStateControl,
                        isFeudal: es.isFeudal,
                        allowsCorporations: es.allowsCorporations,
                        allowsGuilds: es.allowsGuilds,
                        taxationSystemId: es.taxationSystemId
                            ? String(es.taxationSystemId)
                            : "",
                        bankingSystemId: es.bankingSystemId
                            ? String(es.bankingSystemId)
                            : "",
                        historyId: es.historyId ? String(es.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load economic system form data:", err);
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
                await updateEconomicSystem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createEconomicSystem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/economic-systems/${targetId}`);
        } catch (err) {
            console.error("Failed to save economic system:", err);
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
            await deleteEconomicSystem(editId);
            navigate("/storymap/economic-systems");
        } catch (err) {
            console.error("Failed to delete economic system:", err);
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
                glyph="⚜"
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
                        <OrnateField label={t("fields.taxationSystem")}>
                            <OrnateSelect
                                value={form.taxationSystemId}
                                onChange={(e) =>
                                    set("taxationSystemId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {taxationSystems.map((ts) => (
                                    <option key={ts.id} value={ts.id}>
                                        {ts.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("fields.bankingSystem")}>
                            <OrnateSelect
                                value={form.bankingSystemId}
                                onChange={(e) =>
                                    set("bankingSystemId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {bankingSystems.map((bs) => (
                                    <option key={bs.id} value={bs.id}>
                                        {bs.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("fields.isMarketDriven")}
                        checked={form.isMarketDriven}
                        onChange={(e) => set("isMarketDriven", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.hasStateControl")}
                        checked={form.hasStateControl}
                        onChange={(e) =>
                            set("hasStateControl", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.isFeudal")}
                        checked={form.isFeudal}
                        onChange={(e) => set("isFeudal", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.allowsCorporations")}
                        checked={form.allowsCorporations}
                        onChange={(e) =>
                            set("allowsCorporations", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.allowsGuilds")}
                        checked={form.allowsGuilds}
                        onChange={(e) => set("allowsGuilds", e.target.checked)}
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
                                ? `/storymap/economic-systems/${editId}`
                                : "/storymap/economic-systems"
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
