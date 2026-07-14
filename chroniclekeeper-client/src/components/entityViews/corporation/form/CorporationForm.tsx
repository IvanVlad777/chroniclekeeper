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
    createCorporation,
    deleteCorporation,
    getCorporationById,
    getCorporations,
    updateCorporation,
} from "../../../../api/corporations";
import { getIndustries } from "../../../../api/industries";
import { getTaxationSystems } from "../../../../api/taxationSystems";
import { getBankingSystems } from "../../../../api/bankingSystems";
import { getHistories } from "../../../../api/histories";
import {
    BankingSystemDto,
    CorporationDto,
    CorporationUpdateDto,
    HistoryDto,
    IndustryDto,
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
    industrySector: string;
    revenue: string;
    numberOfEmployees: string;
    isPubliclyTraded: boolean;
    isStateOwned: boolean;
    industryId: string;
    taxationSystemId: string;
    bankingSystemId: string;
    parentCorporationId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    industrySector: "",
    revenue: "0",
    numberOfEmployees: "0",
    isPubliclyTraded: false,
    isStateOwned: false,
    industryId: "",
    taxationSystemId: "",
    bankingSystemId: "",
    parentCorporationId: "",
    historyId: "",
};

function toDto(f: FormState): CorporationUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        industrySector: f.industrySector,
        revenue: Number(f.revenue) || 0,
        numberOfEmployees: Number(f.numberOfEmployees) || 0,
        isPubliclyTraded: f.isPubliclyTraded,
        isStateOwned: f.isStateOwned,
        industryId: f.industryId ? Number(f.industryId) : null,
        taxationSystemId: f.taxationSystemId ? Number(f.taxationSystemId) : null,
        bankingSystemId: f.bankingSystemId ? Number(f.bankingSystemId) : null,
        parentCorporationId: f.parentCorporationId
            ? Number(f.parentCorporationId)
            : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /corporations/new i /corporations/:id/edit. */
export default function CorporationForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("corporation");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [industries, setIndustries] = useState<IndustryDto[]>([]);
    const [taxationSystems, setTaxationSystems] = useState<TaxationSystemDto[]>(
        []
    );
    const [bankingSystems, setBankingSystems] = useState<BankingSystemDto[]>([]);
    const [corporations, setCorporations] = useState<CorporationDto[]>([]);
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
            getIndustries(selectedWorld.id),
            getTaxationSystems(selectedWorld.id),
            getBankingSystems(selectedWorld.id),
            getCorporations(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(
                async ([
                    industriesData,
                    taxationData,
                    bankingData,
                    corporationsData,
                    historiesData,
                ]) => {
                    if (cancelled) return;
                    setIndustries(industriesData);
                    setTaxationSystems(taxationData);
                    setBankingSystems(bankingData);
                    setCorporations(corporationsData);
                    setHistories(historiesData);

                    if (isEdit) {
                        const c = await getCorporationById(editId);
                        if (cancelled) return;
                        setForm({
                            name: c.name ?? "",
                            description: c.description ?? "",
                            industrySector: c.industrySector ?? "",
                            revenue: String(c.revenue ?? 0),
                            numberOfEmployees: String(c.numberOfEmployees ?? 0),
                            isPubliclyTraded: c.isPubliclyTraded,
                            isStateOwned: c.isStateOwned,
                            industryId: c.industryId ? String(c.industryId) : "",
                            taxationSystemId: c.taxationSystemId
                                ? String(c.taxationSystemId)
                                : "",
                            bankingSystemId: c.bankingSystemId
                                ? String(c.bankingSystemId)
                                : "",
                            parentCorporationId: c.parentCorporationId
                                ? String(c.parentCorporationId)
                                : "",
                            historyId: c.historyId ? String(c.historyId) : "",
                        });
                    }
                }
            )
            .catch((err) => {
                console.error("Failed to load corporation form data:", err);
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
                await updateCorporation(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createCorporation({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/corporations/${targetId}`);
        } catch (err) {
            console.error("Failed to save corporation:", err);
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
            await deleteCorporation(editId);
            navigate("/storymap/corporations");
        } catch (err) {
            console.error("Failed to delete corporation:", err);
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
                glyph="🏢"
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

    // Vlastita korporacija ne smije biti sama sebi parent
    const parentCandidates = corporations.filter((c) => c.id !== editId);

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
                    <OrnateField label={t("fields.industrySector")}>
                        <OrnateTextInput
                            value={form.industrySector}
                            maxLength={100}
                            placeholder={t("fields.industrySectorHint")}
                            onChange={(e) =>
                                set("industrySector", e.target.value)
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.revenue")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="any"
                                value={form.revenue}
                                onChange={(e) => set("revenue", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.numberOfEmployees")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.numberOfEmployees}
                                onChange={(e) =>
                                    set("numberOfEmployees", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("fields.isPubliclyTraded")}
                        checked={form.isPubliclyTraded}
                        onChange={(e) =>
                            set("isPubliclyTraded", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.isStateOwned")}
                        checked={form.isStateOwned}
                        onChange={(e) => set("isStateOwned", e.target.checked)}
                    />
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.parentCorporation")}>
                        <OrnateSelect
                            value={form.parentCorporationId}
                            onChange={(e) =>
                                set("parentCorporationId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {parentCandidates.map((c) => (
                                <option key={c.id} value={c.id}>
                                    {c.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.industry")}>
                            <OrnateSelect
                                value={form.industryId}
                                onChange={(e) =>
                                    set("industryId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {industries.map((i) => (
                                    <option key={i.id} value={i.id}>
                                        {i.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
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
                    </div>
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
                                ? `/storymap/corporations/${editId}`
                                : "/storymap/corporations"
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
