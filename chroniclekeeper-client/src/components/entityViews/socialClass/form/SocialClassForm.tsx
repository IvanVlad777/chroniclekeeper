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
    createSocialClass,
    deleteSocialClass,
    getSocialClassById,
    updateSocialClass,
} from "../../../../api/socialClasses";
import { SocialClassUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isNoble: boolean;
    isMerchantClass: boolean;
    isOutcast: boolean;
    canOwnLand: boolean;
    canHoldOffice: boolean;
    hasTaxExemptions: boolean;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isNoble: false,
    isMerchantClass: false,
    isOutcast: false,
    canOwnLand: false,
    canHoldOffice: false,
    hasTaxExemptions: false,
};

function toDto(f: FormState): SocialClassUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        isNoble: f.isNoble,
        isMerchantClass: f.isMerchantClass,
        isOutcast: f.isOutcast,
        canOwnLand: f.canOwnLand,
        canHoldOffice: f.canHoldOffice,
        hasTaxExemptions: f.hasTaxExemptions,
    };
}

/** Zajednička forma za /social-classes/new i /social-classes/:id/edit. */
export default function SocialClassForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("socialClass");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

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

        getSocialClassById(editId)
            .then((sc) => {
                if (cancelled) return;
                setForm({
                    name: sc.name ?? "",
                    description: sc.description ?? "",
                    isNoble: sc.isNoble,
                    isMerchantClass: sc.isMerchantClass,
                    isOutcast: sc.isOutcast,
                    canOwnLand: sc.canOwnLand,
                    canHoldOffice: sc.canHoldOffice,
                    hasTaxExemptions: sc.hasTaxExemptions,
                });
            })
            .catch((err) => {
                console.error("Failed to load social class:", err);
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
                await updateSocialClass(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createSocialClass({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/social-classes/${targetId}`);
        } catch (err) {
            console.error("Failed to save social class:", err);
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
            await deleteSocialClass(editId);
            navigate("/storymap/social-classes");
        } catch (err) {
            console.error("Failed to delete social class:", err);
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
                glyph="⚖"
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
                    <span className={s.legend}>{t("form.rightsLegend")}</span>
                    <OrnateCheckbox
                        label={t("fields.isNoble")}
                        checked={form.isNoble}
                        onChange={(e) => set("isNoble", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.isMerchantClass")}
                        checked={form.isMerchantClass}
                        onChange={(e) =>
                            set("isMerchantClass", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.isOutcast")}
                        checked={form.isOutcast}
                        onChange={(e) => set("isOutcast", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.canOwnLand")}
                        checked={form.canOwnLand}
                        onChange={(e) => set("canOwnLand", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.canHoldOffice")}
                        checked={form.canHoldOffice}
                        onChange={(e) =>
                            set("canHoldOffice", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.hasTaxExemptions")}
                        checked={form.hasTaxExemptions}
                        onChange={(e) =>
                            set("hasTaxExemptions", e.target.checked)
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
                                ? `/storymap/social-classes/${editId}`
                                : "/storymap/social-classes"
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
