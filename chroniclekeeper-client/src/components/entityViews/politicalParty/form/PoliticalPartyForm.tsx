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
    createPoliticalParty,
    deletePoliticalParty,
    getPoliticalPartyById,
    updatePoliticalParty,
} from "../../../../api/politicalParties";
import { getPoliticalIdeologies } from "../../../../api/politicalIdeologies";
import { getGovernmentSystems } from "../../../../api/governmentSystems";
import {
    GovernmentSystemDto,
    PoliticalIdeologyDto,
    PoliticalPartyUpdateDto,
    ScaleLevel,
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
    ideologyDescription: string;
    politicalIdeologyId: string;
    governmentSystemId: string;
    influenceLevel: ScaleLevel;
    isBanned: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    ideologyDescription: "",
    politicalIdeologyId: "",
    governmentSystemId: "",
    influenceLevel: "Moderate",
    isBanned: false,
};

const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): PoliticalPartyUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        ideologyDescription: f.ideologyDescription,
        politicalIdeologyId: Number(f.politicalIdeologyId),
        governmentSystemId: toId(f.governmentSystemId),
        influenceLevel: f.influenceLevel,
        isBanned: f.isBanned,
    };
}

/** Zajednička forma za /political-parties/new i /political-parties/:id/edit. */
export default function PoliticalPartyForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("politicalParty");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [ideologies, setIdeologies] = useState<PoliticalIdeologyDto[]>([]);
    const [governmentSystems, setGovernmentSystems] = useState<
        GovernmentSystemDto[]
    >([]);
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
            getPoliticalIdeologies(selectedWorld.id),
            getGovernmentSystems(selectedWorld.id),
        ])
            .then(async ([ideologiesData, governmentSystemsData]) => {
                if (cancelled) return;
                setIdeologies(ideologiesData);
                setGovernmentSystems(governmentSystemsData);

                if (isEdit) {
                    const p = await getPoliticalPartyById(editId);
                    if (cancelled) return;
                    setForm({
                        name: p.name ?? "",
                        description: p.description ?? "",
                        ideologyDescription: p.ideologyDescription ?? "",
                        politicalIdeologyId: p.politicalIdeologyId
                            ? String(p.politicalIdeologyId)
                            : "",
                        governmentSystemId: p.governmentSystemId
                            ? String(p.governmentSystemId)
                            : "",
                        influenceLevel: p.influenceLevel,
                        isBanned: p.isBanned,
                    });
                }
            })
            .catch((err) => {
                console.error(
                    "Failed to load political party form data:",
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
        if (!form.politicalIdeologyId) {
            setSaveError(t("form.ideologyMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updatePoliticalParty(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createPoliticalParty({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/political-parties/${targetId}`);
        } catch (err) {
            console.error("Failed to save political party:", err);
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
            await deletePoliticalParty(editId);
            navigate("/storymap/political-parties");
        } catch (err) {
            console.error("Failed to delete political party:", err);
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
                glyph="✪"
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
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.ideologyDescription")}>
                        <OrnateTextArea
                            value={form.ideologyDescription}
                            rows={6}
                            maxLength={2000}
                            onChange={(e) =>
                                set("ideologyDescription", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.politicalIdeology")} required>
                        <OrnateSelect
                            value={form.politicalIdeologyId}
                            onChange={(e) =>
                                set("politicalIdeologyId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {ideologies.map((i) => (
                                <option key={i.id} value={i.id}>
                                    {i.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.governmentSystem")}>
                        <OrnateSelect
                            value={form.governmentSystemId}
                            onChange={(e) =>
                                set("governmentSystemId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {governmentSystems.map((g) => (
                                <option key={g.id} value={g.id}>
                                    {g.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.influenceLevel")}>
                        <OrnateSelect
                            value={form.influenceLevel}
                            onChange={(e) =>
                                set(
                                    "influenceLevel",
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
                    <OrnateCheckbox
                        label={t("fields.isBanned")}
                        checked={form.isBanned}
                        onChange={(e) => set("isBanned", e.target.checked)}
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
                                ? `/storymap/political-parties/${editId}`
                                : "/storymap/political-parties"
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
