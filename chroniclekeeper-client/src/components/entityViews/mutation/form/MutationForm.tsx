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
    createMutation,
    deleteMutation,
    getMutationById,
    updateMutation,
} from "../../../../api/mutations";
import { getCreatures } from "../../../../api/creatures";
import { getHistories } from "../../../../api/histories";
import {
    CreatureDto,
    HistoryDto,
    MutationEffect,
    MutationOrigin,
    MutationUpdateDto,
    mutationEffects,
    mutationOrigins,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    origin: MutationOrigin;
    effect: MutationEffect;
    mutantCreatureId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    origin: "Radiation",
    effect: "Beneficial",
    mutantCreatureId: "",
    historyId: "",
};

function toDto(f: FormState): MutationUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        origin: f.origin,
        effect: f.effect,
        mutantCreatureId: f.mutantCreatureId
            ? Number(f.mutantCreatureId)
            : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /mutations/new i /:id/edit. */
export default function MutationForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("mutation");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [creatures, setCreatures] = useState<CreatureDto[]>([]);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
    const [loading, setLoading] = useState(isEdit);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (!selectedWorld) return;
        getCreatures({ worldId: selectedWorld.id })
            .then(setCreatures)
            .catch((err) => console.error("Failed to load creatures:", err));
        getHistories(selectedWorld.id)
            .then(setHistories)
            .catch((err) => console.error("Failed to load histories:", err));
    }, [selectedWorld]);

    useEffect(() => {
        if (!isEdit) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getMutationById(editId)
            .then((m) => {
                if (cancelled) return;
                setForm({
                    name: m.name ?? "",
                    description: m.description ?? "",
                    origin: m.origin,
                    effect: m.effect,
                    mutantCreatureId: m.mutantCreatureId
                        ? String(m.mutantCreatureId)
                        : "",
                    historyId: m.historyId ? String(m.historyId) : "",
                });
            })
            .catch((err) => {
                console.error("Failed to load mutation:", err);
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
                await updateMutation(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createMutation({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/mutations/${targetId}`);
        } catch (err) {
            console.error("Failed to save mutation:", err);
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
            await deleteMutation(editId);
            navigate("/storymap/mutations");
        } catch (err) {
            console.error("Failed to delete mutation:", err);
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
                glyph="🧬"
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
                            rows={8}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.origin")}>
                        <OrnateSelect
                            value={form.origin}
                            onChange={(e) =>
                                set("origin", e.target.value as MutationOrigin)
                            }
                        >
                            {mutationOrigins.map((o) => (
                                <option key={o} value={o}>
                                    {t(`origins.${o}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.effect")}>
                        <OrnateSelect
                            value={form.effect}
                            onChange={(e) =>
                                set("effect", e.target.value as MutationEffect)
                            }
                        >
                            {mutationEffects.map((o) => (
                                <option key={o} value={o}>
                                    {t(`effects.${o}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.mutantCreature")}>
                        <OrnateSelect
                            value={form.mutantCreatureId}
                            onChange={(e) =>
                                set("mutantCreatureId", e.target.value)
                            }
                        >
                            <option value="">{t("form.noCreature")}</option>
                            {creatures.map((c) => (
                                <option key={c.id} value={c.id}>
                                    {c.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.history")}>
                        <OrnateSelect
                            value={form.historyId}
                            onChange={(e) => set("historyId", e.target.value)}
                        >
                            <option value="">{t("form.noHistory")}</option>
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
                    <Button
                        variant="danger"
                        disabled={busy}
                        onClick={onDelete}
                    >
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
                                ? `/storymap/mutations/${editId}`
                                : "/storymap/mutations"
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
