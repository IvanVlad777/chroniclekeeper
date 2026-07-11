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
    createLibrary,
    deleteLibrary,
    getLibraryById,
    updateLibrary,
} from "../../../../api/libraries";
import { getUniversities } from "../../../../api/universities";
import { getLocations } from "../../../../api/locations";
import {
    LocationDto,
    UniversityDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    isPublic: boolean;
    focusesOnMagic: boolean;
    focusesOnHistory: boolean;
    universityId: string;
    locationId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    isPublic: false,
    focusesOnMagic: false,
    focusesOnHistory: false,
    universityId: "",
    locationId: "",
};

/** Zajednička forma za /libraries/new i /libraries/:id/edit. */
export default function LibraryForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("library");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [universities, setUniversities] = useState<UniversityDto[]>([]);
    const [locations, setLocations] = useState<LocationDto[]>([]);
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
            getUniversities({ worldId: selectedWorld.id }),
            getLocations(selectedWorld.id),
            isEdit ? getLibraryById(editId) : Promise.resolve(null),
        ])
            .then(([universitiesData, locationsData, library]) => {
                if (cancelled) return;
                setUniversities(universitiesData);
                setLocations(locationsData);
                if (library) {
                    setForm({
                        name: library.name ?? "",
                        description: library.description ?? "",
                        isPublic: library.isPublic,
                        focusesOnMagic: library.focusesOnMagic,
                        focusesOnHistory: library.focusesOnHistory,
                        universityId: library.universityId
                            ? String(library.universityId)
                            : "",
                        locationId: library.locationId
                            ? String(library.locationId)
                            : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load library form data:", err);
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
            const payload = {
                name: form.name.trim(),
                description: form.description,
                isPublic: form.isPublic,
                focusesOnMagic: form.focusesOnMagic,
                focusesOnHistory: form.focusesOnHistory,
                universityId: form.universityId
                    ? Number(form.universityId)
                    : null,
                locationId: form.locationId ? Number(form.locationId) : null,
            };
            if (isEdit) {
                await updateLibrary(editId, payload);
                targetId = editId;
            } else {
                const created = await createLibrary({
                    ...payload,
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/libraries/${targetId}`);
        } catch (err) {
            console.error("Failed to save library:", err);
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
            await deleteLibrary(editId);
            navigate("/storymap/libraries");
        } catch (err) {
            console.error("Failed to delete library:", err);
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
                glyph="❖"
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
                    <OrnateField label={t("form.university")}>
                        <OrnateSelect
                            value={form.universityId}
                            onChange={(e) =>
                                set("universityId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {universities.map((u) => (
                                <option key={u.id} value={u.id}>
                                    {u.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("form.location")}>
                        <OrnateSelect
                            value={form.locationId}
                            onChange={(e) =>
                                set("locationId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {locations.map((l) => (
                                <option key={l.id} value={l.id}>
                                    {l.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isPublic")}
                        checked={form.isPublic}
                        onChange={(e) => set("isPublic", e.target.checked)}
                    />
                    <OrnateCheckbox
                        label={t("fields.focusesOnMagic")}
                        checked={form.focusesOnMagic}
                        onChange={(e) =>
                            set("focusesOnMagic", e.target.checked)
                        }
                    />
                    <OrnateCheckbox
                        label={t("fields.focusesOnHistory")}
                        checked={form.focusesOnHistory}
                        onChange={(e) =>
                            set("focusesOnHistory", e.target.checked)
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
                                ? `/storymap/libraries/${editId}`
                                : "/storymap/libraries"
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
