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
    createExtractionMethod,
    deleteExtractionMethod,
    getExtractionMethodById,
    updateExtractionMethod,
} from "../../../../api/extractionMethods";
import { getHistories } from "../../../../api/histories";
import {
    ExtractionMethodUpdateDto,
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
    methodType: string;
    isSustainable: boolean;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    methodType: "",
    isSustainable: false,
    historyId: "",
};

function toDto(f: FormState): ExtractionMethodUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        methodType: f.methodType,
        isSustainable: f.isSustainable,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /extraction-methods/new i /extraction-methods/:id/edit. */
export default function ExtractionMethodForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("extractionMethod");
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
                    const m = await getExtractionMethodById(editId);
                    if (cancelled) return;
                    setForm({
                        name: m.name ?? "",
                        description: m.description ?? "",
                        methodType: m.methodType ?? "",
                        isSustainable: m.isSustainable,
                        historyId: m.historyId ? String(m.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load extraction method form data:", err);
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
                await updateExtractionMethod(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createExtractionMethod({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/extraction-methods/${targetId}`);
        } catch (err) {
            console.error("Failed to save extraction method:", err);
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
            await deleteExtractionMethod(editId);
            navigate("/storymap/extraction-methods");
        } catch (err) {
            console.error("Failed to delete extraction method:", err);
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
                glyph="⛏"
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
                    <OrnateField label={t("fields.methodType")}>
                        <OrnateTextInput
                            value={form.methodType}
                            maxLength={100}
                            placeholder={t("fields.methodTypeHint")}
                            onChange={(e) => set("methodType", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isSustainable")}
                        checked={form.isSustainable}
                        onChange={(e) => set("isSustainable", e.target.checked)}
                    />
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
                                ? `/storymap/extraction-methods/${editId}`
                                : "/storymap/extraction-methods"
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
