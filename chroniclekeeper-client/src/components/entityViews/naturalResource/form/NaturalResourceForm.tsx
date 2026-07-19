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
    createNaturalResource,
    deleteNaturalResource,
    getNaturalResourceById,
    updateNaturalResource,
} from "../../../../api/naturalResources";
import { getExtractionMethods } from "../../../../api/extractionMethods";
import { getHistories } from "../../../../api/histories";
import {
    NaturalResourceUpdateDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    resourceType: string;
    quantity: string;
    marketValue: string;
    isRenewable: boolean;
    isStrategicResource: boolean;
    extractionMethodId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    resourceType: "",
    quantity: "0",
    marketValue: "0",
    isRenewable: false,
    isStrategicResource: false,
    extractionMethodId: "",
    historyId: "",
};

function toDto(f: FormState): NaturalResourceUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        resourceType: f.resourceType,
        quantity: Number(f.quantity) || 0,
        marketValue: Number(f.marketValue) || 0,
        isRenewable: f.isRenewable,
        isStrategicResource: f.isStrategicResource,
        extractionMethodId: f.extractionMethodId
            ? Number(f.extractionMethodId)
            : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /natural-resources/new i /natural-resources/:id/edit. */
export default function NaturalResourceForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("naturalResource");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [extractionMethodOptions, setExtractionMethodOptions] = useState<
        EntityOption[]
    >([]);
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
            getExtractionMethods(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(async ([methodsData, historiesData]) => {
                if (cancelled) return;
                setExtractionMethodOptions(
                    methodsData.map((m) => ({ value: m.id, label: m.name }))
                );
                setHistoryOptions(
                    historiesData.map((h) => ({ value: h.id, label: h.name }))
                );

                if (isEdit) {
                    const nr = await getNaturalResourceById(editId);
                    if (cancelled) return;
                    setForm({
                        name: nr.name ?? "",
                        description: nr.description ?? "",
                        resourceType: nr.resourceType ?? "",
                        quantity: String(nr.quantity ?? 0),
                        marketValue: String(nr.marketValue ?? 0),
                        isRenewable: nr.isRenewable,
                        isStrategicResource: nr.isStrategicResource,
                        extractionMethodId: nr.extractionMethodId
                            ? String(nr.extractionMethodId)
                            : "",
                        historyId: nr.historyId ? String(nr.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load natural resource form data:", err);
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
                await updateNaturalResource(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createNaturalResource({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/natural-resources/${targetId}`);
        } catch (err) {
            console.error("Failed to save natural resource:", err);
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
            await deleteNaturalResource(editId);
            navigate("/storymap/natural-resources");
        } catch (err) {
            console.error("Failed to delete natural resource:", err);
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
                glyph="⛰"
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

                <div className={s.col}>
                    <OrnateField label={t("fields.resourceType")}>
                        <OrnateTextInput
                            value={form.resourceType}
                            maxLength={100}
                            placeholder={t("fields.resourceTypeHint")}
                            onChange={(e) => set("resourceType", e.target.value)}
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.quantity")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="any"
                                value={form.quantity}
                                onChange={(e) => set("quantity", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.marketValue")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="any"
                                value={form.marketValue}
                                onChange={(e) =>
                                    set("marketValue", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.extractionMethod")}>
                        <EntityPicker
                            kind="extractionMethod"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.extractionMethodId}
                            options={extractionMethodOptions}
                            onChange={(v) => set("extractionMethodId", v)}
                            onCreated={(m) =>
                                setExtractionMethodOptions((prev) => [
                                    ...prev,
                                    { value: m.id, label: m.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isRenewable")}
                        checked={form.isRenewable}
                        onChange={(e) => set("isRenewable", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isStrategicResource")}
                        checked={form.isStrategicResource}
                        onChange={(e) =>
                            set("isStrategicResource", e.target.checked)
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
                                ? `/storymap/natural-resources/${editId}`
                                : "/storymap/natural-resources"
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
