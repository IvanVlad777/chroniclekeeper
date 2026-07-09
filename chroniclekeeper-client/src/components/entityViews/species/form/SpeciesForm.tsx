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
    createSpecies,
    deleteSpecies,
    getSpeciesById,
    updateSpecies,
} from "../../../../api/species";
import { SpeciesUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    commonName: string;
    scientificName: string;
    isHumanoid: boolean;
    lifespan: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    commonName: "",
    scientificName: "",
    isHumanoid: true,
    lifespan: "",
};

function toDto(f: FormState): SpeciesUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        commonName: f.commonName.trim(),
        scientificName: f.scientificName.trim(),
        isHumanoid: f.isHumanoid,
        lifespan: f.lifespan.trim(),
    };
}

/** Zajednička forma za /species/new i /species/:id/edit. */
export default function SpeciesForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("species");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [form, setForm] = useState<FormState>(emptyForm);
    const [loading, setLoading] = useState(isEdit);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (!isEdit) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getSpeciesById(editId)
            .then((sp) => {
                if (cancelled) return;
                setForm({
                    name: sp.name ?? "",
                    description: sp.description ?? "",
                    commonName: sp.commonName ?? "",
                    scientificName: sp.scientificName ?? "",
                    isHumanoid: sp.isHumanoid,
                    lifespan: sp.lifespan ?? "",
                });
            })
            .catch((err) => {
                console.error("Failed to load species:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [isEdit, editId, t, reloadKey]);

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
                await updateSpecies(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createSpecies({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/species/${targetId}`);
        } catch (err) {
            console.error("Failed to save species:", err);
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
            await deleteSpecies(editId);
            navigate("/storymap/species");
        } catch (err) {
            console.error("Failed to delete species:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⚘"
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
                            rows={10}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.commonName")}>
                        <OrnateTextInput
                            value={form.commonName}
                            maxLength={100}
                            onChange={(e) =>
                                set("commonName", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.scientificName")}>
                        <OrnateTextInput
                            value={form.scientificName}
                            maxLength={100}
                            onChange={(e) =>
                                set("scientificName", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField
                        label={t("fields.lifespan")}
                        hint={t("form.lifespanHint")}
                    >
                        <OrnateTextInput
                            value={form.lifespan}
                            maxLength={100}
                            onChange={(e) => set("lifespan", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isHumanoid")}
                        checked={form.isHumanoid}
                        onChange={(e) => set("isHumanoid", e.target.checked)}
                    />
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && (
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
                                ? `/storymap/species/${editId}`
                                : "/storymap/species"
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
