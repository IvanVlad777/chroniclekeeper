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
    createDiplomaticAgreement,
    deleteDiplomaticAgreement,
    getDiplomaticAgreementById,
    updateDiplomaticAgreement,
} from "../../../../api/diplomaticAgreements";
import { getNations } from "../../../../api/nations";
import {
    DiplomaticAgreementUpdateDto,
    NationDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    agreementType: string;
    signedDate: string;
    terminationDate: string;
    durationYears: string;
    terms: string;
    isUnequal: boolean;
    firstNationId: string;
    secondNationId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    agreementType: "",
    signedDate: "",
    terminationDate: "",
    durationYears: "",
    terms: "",
    isUnequal: false,
    firstNationId: "",
    secondNationId: "",
};

const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): DiplomaticAgreementUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        agreementType: f.agreementType,
        signedDate: f.signedDate,
        terminationDate: f.terminationDate || null,
        durationYears: toId(f.durationYears),
        terms: f.terms,
        isUnequal: f.isUnequal,
        firstNationId: Number(f.firstNationId),
        secondNationId: Number(f.secondNationId),
    };
}

/** Zajednička forma za /diplomatic-agreements/new i /diplomatic-agreements/:id/edit. */
export default function DiplomaticAgreementForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("diplomaticAgreement");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [nations, setNations] = useState<NationDto[]>([]);
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

        getNations(selectedWorld.id)
            .then(async (nationsData) => {
                if (cancelled) return;
                setNations(nationsData);

                if (isEdit) {
                    const a = await getDiplomaticAgreementById(editId);
                    if (cancelled) return;
                    setForm({
                        name: a.name ?? "",
                        description: a.description ?? "",
                        agreementType: a.agreementType ?? "",
                        signedDate: a.signedDate ?? "",
                        terminationDate: a.terminationDate ?? "",
                        durationYears:
                            a.durationYears != null
                                ? String(a.durationYears)
                                : "",
                        terms: a.terms ?? "",
                        isUnequal: a.isUnequal,
                        firstNationId: String(a.firstNationId),
                        secondNationId: String(a.secondNationId),
                    });
                }
            })
            .catch((err) => {
                console.error(
                    "Failed to load diplomatic agreement form data:",
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
        if (!form.firstNationId || !form.secondNationId) {
            setSaveError(t("form.nationsMissing"));
            return;
        }
        if (form.firstNationId === form.secondNationId) {
            setSaveError(t("form.sameNation"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateDiplomaticAgreement(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createDiplomaticAgreement({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/diplomatic-agreements/${targetId}`);
        } catch (err) {
            console.error("Failed to save diplomatic agreement:", err);
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
            await deleteDiplomaticAgreement(editId);
            navigate("/storymap/diplomatic-agreements");
        } catch (err) {
            console.error("Failed to delete diplomatic agreement:", err);
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
                glyph="⚜"
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
                    <OrnateField label={t("fields.terms")}>
                        <OrnateTextArea
                            value={form.terms}
                            rows={8}
                            maxLength={4000}
                            onChange={(e) => set("terms", e.target.value)}
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.firstNation")} required>
                        <OrnateSelect
                            value={form.firstNationId}
                            onChange={(e) =>
                                set("firstNationId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {nations.map((n) => (
                                <option key={n.id} value={n.id}>
                                    {n.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.secondNation")} required>
                        <OrnateSelect
                            value={form.secondNationId}
                            onChange={(e) =>
                                set("secondNationId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {nations.map((n) => (
                                <option key={n.id} value={n.id}>
                                    {n.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.agreementType")}>
                        <OrnateTextInput
                            value={form.agreementType}
                            maxLength={100}
                            onChange={(e) =>
                                set("agreementType", e.target.value)
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.signedDate")}>
                            <OrnateTextInput
                                value={form.signedDate}
                                maxLength={100}
                                onChange={(e) =>
                                    set("signedDate", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.terminationDate")}>
                            <OrnateTextInput
                                value={form.terminationDate}
                                maxLength={100}
                                onChange={(e) =>
                                    set("terminationDate", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.durationYears")}>
                        <OrnateTextInput
                            type="number"
                            min={0}
                            value={form.durationYears}
                            onChange={(e) =>
                                set("durationYears", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isUnequal")}
                        checked={form.isUnequal}
                        onChange={(e) => set("isUnequal", e.target.checked)}
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
                                ? `/storymap/diplomatic-agreements/${editId}`
                                : "/storymap/diplomatic-agreements"
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
