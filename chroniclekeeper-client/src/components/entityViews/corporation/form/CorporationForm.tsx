import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
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
import { CorporationUpdateDto } from "../../../../interfaces/loreInterfaces";
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
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [industryOptions, setIndustryOptions] = useState<EntityOption[]>([]);
    const [taxationSystemOptions, setTaxationSystemOptions] = useState<
        EntityOption[]
    >([]);
    const [bankingSystemOptions, setBankingSystemOptions] = useState<
        EntityOption[]
    >([]);
    const [corporationOptions, setCorporationOptions] = useState<EntityOption[]>(
        []
    );
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
                    setIndustryOptions(
                        industriesData.map((i) => ({
                            value: i.id,
                            label: i.name,
                        }))
                    );
                    setTaxationSystemOptions(
                        taxationData.map((ts) => ({
                            value: ts.id,
                            label: ts.name,
                        }))
                    );
                    setBankingSystemOptions(
                        bankingData.map((bs) => ({
                            value: bs.id,
                            label: bs.name,
                        }))
                    );
                    setCorporationOptions(
                        corporationsData.map((c) => ({
                            value: c.id,
                            label: c.name,
                        }))
                    );
                    setHistoryOptions(
                        historiesData.map((h) => ({
                            value: h.id,
                            label: h.name,
                        }))
                    );

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
                        <EntityPicker
                            kind="corporation"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.parentCorporationId}
                            options={corporationOptions}
                            excludeValue={editId ?? undefined}
                            onChange={(v) => set("parentCorporationId", v)}
                            onCreated={(c) =>
                                setCorporationOptions((prev) => [
                                    ...prev,
                                    { value: c.id, label: c.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.industry")}>
                            <EntityPicker
                                kind="industry"
                                worldId={selectedWorld.id}
                                canCreate={canCreate}
                                noneLabel={t("none")}
                                value={form.industryId}
                                options={industryOptions}
                                onChange={(v) => set("industryId", v)}
                                onCreated={(i) =>
                                    setIndustryOptions((prev) => [
                                        ...prev,
                                        { value: i.id, label: i.name },
                                    ])
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.taxationSystem")}>
                            <EntityPicker
                                kind="taxationSystem"
                                worldId={selectedWorld.id}
                                canCreate={canCreate}
                                noneLabel={t("none")}
                                value={form.taxationSystemId}
                                options={taxationSystemOptions}
                                onChange={(v) => set("taxationSystemId", v)}
                                onCreated={(ts) =>
                                    setTaxationSystemOptions((prev) => [
                                        ...prev,
                                        { value: ts.id, label: ts.name },
                                    ])
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.bankingSystem")}>
                        <EntityPicker
                            kind="bankingSystem"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.bankingSystemId}
                            options={bankingSystemOptions}
                            onChange={(v) => set("bankingSystemId", v)}
                            onCreated={(bs) =>
                                setBankingSystemOptions((prev) => [
                                    ...prev,
                                    { value: bs.id, label: bs.name },
                                ])
                            }
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
