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
    createFaction,
    deleteFaction,
    getFaction,
    updateFaction,
} from "../../../../api/factions";
import { getCharacters } from "../../../../api/characters";
import { getLocations } from "../../../../api/locations";
import {
    FactionType,
    FactionUpdateDto,
    factionTypes,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { EntityPicker, type EntityOption } from "../../../quickCreate/EntityPicker";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    type: FactionType;
    isSecretive: boolean;
    motto: string;
    leaderId: string;
    headquartersId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    type: "Adventurers",
    isSecretive: false,
    motto: "",
    leaderId: "",
    headquartersId: "",
    historyId: "",
};

const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): FactionUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        type: f.type,
        isSecretive: f.isSecretive,
        motto: f.motto.trim(),
        leaderId: toId(f.leaderId),
        headquartersId: toId(f.headquartersId),
        historyId: toId(f.historyId),
    };
}

/** Zajednička forma za /factions/new i /factions/:id/edit. */
export default function FactionForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("faction");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [characterOptions, setCharacterOptions] = useState<EntityOption[]>([]);
    const [locationOptions, setLocationOptions] = useState<EntityOption[]>([]);
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
            getCharacters(selectedWorld.id),
            getLocations(selectedWorld.id),
        ])
            .then(async ([chars, locs]) => {
                if (cancelled) return;
                setCharacterOptions(chars.map((c) => ({ value: c.id, label: c.name })));
                setLocationOptions(locs.map((l) => ({ value: l.id, label: l.name })));
                if (isEdit) {
                    const f = await getFaction(editId);
                    if (cancelled) return;
                    setForm({
                        name: f.name ?? "",
                        description: f.description ?? "",
                        type: f.type,
                        isSecretive: f.isSecretive,
                        motto: f.motto ?? "",
                        leaderId: f.leaderId ? String(f.leaderId) : "",
                        headquartersId: f.headquartersId
                            ? String(f.headquartersId)
                            : "",
                        historyId: f.historyId ? String(f.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load faction form data:", err);
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
                await updateFaction(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createFaction({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/factions/${targetId}`);
        } catch (err) {
            console.error("Failed to save faction:", err);
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
            await deleteFaction(editId);
            navigate("/storymap/factions");
        } catch (err) {
            console.error("Failed to delete faction:", err);
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
                glyph="⚔"
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
                    <OrnateField label={t("fields.motto")}>
                        <OrnateTextInput
                            value={form.motto}
                            maxLength={200}
                            onChange={(e) => set("motto", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={9}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.type")}>
                        <OrnateSelect
                            value={form.type}
                            onChange={(e) =>
                                set("type", e.target.value as FactionType)
                            }
                        >
                            {factionTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`types.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.leader")}>
                        <EntityPicker
                            kind="character"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.leaderId}
                            options={characterOptions}
                            onChange={(v) => set("leaderId", v)}
                            onCreated={(c) =>
                                setCharacterOptions((prev) => [
                                    ...prev,
                                    { value: c.id, label: c.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.headquarters")}>
                        <EntityPicker
                            kind="location"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.headquartersId}
                            options={locationOptions}
                            onChange={(v) => set("headquartersId", v)}
                            onCreated={(l) =>
                                setLocationOptions((prev) => [
                                    ...prev,
                                    { value: l.id, label: l.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isSecretive")}
                        checked={form.isSecretive}
                        onChange={(e) => set("isSecretive", e.target.checked)}
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
                                ? `/storymap/factions/${editId}`
                                : "/storymap/factions"
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
