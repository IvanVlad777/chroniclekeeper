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
    createArmy,
    deleteArmy,
    getArmyById,
    updateArmy,
} from "../../../../api/armies";
import { getMilitaryOrganizations } from "../../../../api/militaryOrganizations";
import { getFactions } from "../../../../api/factions";
import { getHistories } from "../../../../api/histories";
import { ArmyUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { EntityPicker, type EntityOption } from "../../../quickCreate/EntityPicker";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    isStandingArmy: boolean;
    size: string;
    militaryOrganizationId: string;
    factionId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isStandingArmy: false,
    size: "0",
    militaryOrganizationId: "",
    factionId: "",
    historyId: "",
};

function toDto(f: FormState): ArmyUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isStandingArmy: f.isStandingArmy,
        size: Number(f.size) || 0,
        cityId: null,
        militaryOrganizationId: f.militaryOrganizationId
            ? Number(f.militaryOrganizationId)
            : null,
        factionId: f.factionId ? Number(f.factionId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function ArmyForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("army");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [organizationOptions, setOrganizationOptions] = useState<
        EntityOption[]
    >([]);
    const [factionOptions, setFactionOptions] = useState<EntityOption[]>([]);
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
            getMilitaryOrganizations(selectedWorld.id),
            getFactions(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(async ([orgs, factions, historiesData]) => {
                if (cancelled) return;
                setOrganizationOptions(
                    orgs.map((o) => ({ value: o.id, label: o.name }))
                );
                setFactionOptions(
                    factions.map((f) => ({ value: f.id, label: f.name }))
                );
                setHistoryOptions(
                    historiesData.map((h) => ({ value: h.id, label: h.name }))
                );
                if (isEdit) {
                    const a = await getArmyById(editId);
                    if (cancelled) return;
                    setForm({
                        name: a.name ?? "",
                        description: a.description ?? "",
                        isStandingArmy: a.isStandingArmy,
                        size: String(a.size ?? 0),
                        militaryOrganizationId: a.militaryOrganizationId
                            ? String(a.militaryOrganizationId)
                            : "",
                        factionId: a.factionId ? String(a.factionId) : "",
                        historyId: a.historyId ? String(a.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load army form data:", err);
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
                await updateArmy(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createArmy({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/armies/${targetId}`);
        } catch (err) {
            console.error("Failed to save army:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) return;
        setSaveError(null);
        setBusy(true);
        try {
            await deleteArmy(editId);
            navigate("/storymap/armies");
        } catch (err) {
            console.error("Failed to delete army:", err);
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
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={7}
                            maxLength={4000}
                            onChange={(e) => set("description", e.target.value)}
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.size")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.size}
                                onChange={(e) => set("size", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.isStandingArmy")}>
                            <OrnateCheckbox
                                label={t("fields.isStandingArmy")}
                                checked={form.isStandingArmy}
                                onChange={(e) =>
                                    set("isStandingArmy", e.target.checked)
                                }
                            />
                        </OrnateField>
                    </div>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.militaryOrganization")}>
                        <EntityPicker
                            kind="militaryOrganization"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.militaryOrganizationId}
                            options={organizationOptions}
                            onChange={(v) => set("militaryOrganizationId", v)}
                            onCreated={(o) =>
                                setOrganizationOptions((prev) => [
                                    ...prev,
                                    { value: o.id, label: o.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.faction")}>
                        <EntityPicker
                            kind="faction"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.factionId}
                            options={factionOptions}
                            onChange={(v) => set("factionId", v)}
                            onCreated={(f) =>
                                setFactionOptions((prev) => [
                                    ...prev,
                                    { value: f.id, label: f.name },
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
                {isEdit && canCreate && (
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
                                ? `/storymap/armies/${editId}`
                                : "/storymap/armies"
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
