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
    EntityPicker,
    type EntityOption,
} from "../../../quickCreate/EntityPicker";
import {
    createGovernmentSystem,
    deleteGovernmentSystem,
    getGovernmentSystemById,
    updateGovernmentSystem,
} from "../../../../api/governmentSystems";
import { getPoliticalIdeologies } from "../../../../api/politicalIdeologies";
import {
    ElectionSystem,
    GovernmentSystemUpdateDto,
    ScaleLevel,
    electionSystems,
    scaleLevels,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isDemocratic: boolean;
    isMonarchic: boolean;
    isReligious: boolean;
    isFederal: boolean;
    isCentralized: boolean;
    politicalIdeologyId: string;
    electionSystem: ElectionSystem;
    stabilityLevel: ScaleLevel;
    hasTermLimits: boolean;
    maxTermLength: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isDemocratic: false,
    isMonarchic: false,
    isReligious: false,
    isFederal: false,
    isCentralized: false,
    politicalIdeologyId: "",
    electionSystem: "DirectElection",
    stabilityLevel: "Moderate",
    hasTermLimits: false,
    maxTermLength: "",
};

const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): GovernmentSystemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isDemocratic: f.isDemocratic,
        isMonarchic: f.isMonarchic,
        isReligious: f.isReligious,
        isFederal: f.isFederal,
        isCentralized: f.isCentralized,
        politicalIdeologyId: toId(f.politicalIdeologyId),
        electionSystem: f.electionSystem,
        stabilityLevel: f.stabilityLevel,
        hasTermLimits: f.hasTermLimits,
        maxTermLength: f.hasTermLimits ? toId(f.maxTermLength) : null,
    };
}

/** Zajednička forma za /government-systems/new i /government-systems/:id/edit. */
export default function GovernmentSystemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("governmentSystem");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [ideologyOptions, setIdeologyOptions] = useState<EntityOption[]>([]);
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

        Promise.all([getPoliticalIdeologies(selectedWorld.id)])
            .then(async ([ideologiesData]) => {
                if (cancelled) return;
                setIdeologyOptions(
                    ideologiesData.map((i) => ({ value: i.id, label: i.name }))
                );

                if (isEdit) {
                    const g = await getGovernmentSystemById(editId);
                    if (cancelled) return;
                    setForm({
                        name: g.name ?? "",
                        description: g.description ?? "",
                        isDemocratic: g.isDemocratic,
                        isMonarchic: g.isMonarchic,
                        isReligious: g.isReligious,
                        isFederal: g.isFederal,
                        isCentralized: g.isCentralized,
                        politicalIdeologyId: g.politicalIdeologyId
                            ? String(g.politicalIdeologyId)
                            : "",
                        electionSystem: g.electionSystem,
                        stabilityLevel: g.stabilityLevel,
                        hasTermLimits: g.hasTermLimits,
                        maxTermLength:
                            g.maxTermLength != null
                                ? String(g.maxTermLength)
                                : "",
                    });
                }
            })
            .catch((err) => {
                console.error(
                    "Failed to load government system form data:",
                    err
                );
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
                await updateGovernmentSystem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createGovernmentSystem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/government-systems/${targetId}`);
        } catch (err) {
            console.error("Failed to save government system:", err);
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
            await deleteGovernmentSystem(editId);
            navigate("/storymap/government-systems");
        } catch (err) {
            console.error("Failed to delete government system:", err);
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
                glyph="⌂"
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
                    <OrnateField label={t("fields.politicalIdeology")}>
                        <EntityPicker
                            kind="politicalIdeology"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.politicalIdeologyId}
                            options={ideologyOptions}
                            onChange={(v) => set("politicalIdeologyId", v)}
                            onCreated={(i) =>
                                setIdeologyOptions((prev) => [
                                    ...prev,
                                    { value: i.id, label: i.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.electionSystem")}>
                            <OrnateSelect
                                value={form.electionSystem}
                                onChange={(e) =>
                                    set(
                                        "electionSystem",
                                        e.target.value as ElectionSystem
                                    )
                                }
                            >
                                {electionSystems.map((es) => (
                                    <option key={es} value={es}>
                                        {t(`electionSystems.${es}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("fields.stabilityLevel")}>
                            <OrnateSelect
                                value={form.stabilityLevel}
                                onChange={(e) =>
                                    set(
                                        "stabilityLevel",
                                        e.target.value as ScaleLevel
                                    )
                                }
                            >
                                {scaleLevels.map((level) => (
                                    <option key={level} value={level}>
                                        {t(`scaleLevels.${level}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                </div>

                <div className={s.col}>
                    <span className={s.legend}>{t("form.traitsLegend")}</span>
                    <OrnateCheckbox
                        label={t("fields.isDemocratic")}
                        checked={form.isDemocratic}
                        onChange={(e) =>
                            set("isDemocratic", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.isMonarchic")}
                        checked={form.isMonarchic}
                        onChange={(e) => set("isMonarchic", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isReligious")}
                        checked={form.isReligious}
                        onChange={(e) => set("isReligious", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isFederal")}
                        checked={form.isFederal}
                        onChange={(e) => set("isFederal", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isCentralized")}
                        checked={form.isCentralized}
                        onChange={(e) =>
                            set("isCentralized", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.hasTermLimits")}
                        checked={form.hasTermLimits}
                        onChange={(e) =>
                            set("hasTermLimits", e.target.checked)
                        }
                    />
                    {form.hasTermLimits && (
                        <OrnateField label={t("fields.maxTermLength")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.maxTermLength}
                                onChange={(e) =>
                                    set("maxTermLength", e.target.value)
                                }
                            />
                        </OrnateField>
                    )}
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
                                ? `/storymap/government-systems/${editId}`
                                : "/storymap/government-systems"
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
