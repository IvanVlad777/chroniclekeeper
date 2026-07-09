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
    createLocation,
    deleteLocation,
    getLocation,
    getLocations,
    updateLocation,
} from "../../../../api/locations";
import {
    LocationDto,
    LocationType,
    LocationUpdateDto,
    locationTypes,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface FormState {
    name: string;
    description: string;
    type: LocationType;
    area: string;
    population: string;
    latitude: string;
    longitude: string;
    parentLocationId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    type: "Other",
    area: "",
    population: "",
    latitude: "",
    longitude: "",
    parentLocationId: "",
};

const toNum = (v: string): number | null => (v.trim() ? Number(v) : null);
const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): LocationUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        type: f.type,
        area: toNum(f.area),
        population: toNum(f.population),
        latitude: toNum(f.latitude),
        longitude: toNum(f.longitude),
        parentLocationId: toId(f.parentLocationId),
    };
}

/** Zajednička forma za /locations/new i /locations/:id/edit. */
export default function LocationForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("location");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [form, setForm] = useState<FormState>(emptyForm);
    const [worldLocations, setWorldLocations] = useState<LocationDto[]>([]);
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

        getLocations(selectedWorld.id)
            .then(async (locations) => {
                if (cancelled) return;
                setWorldLocations(locations);
                if (isEdit) {
                    const l = await getLocation(editId);
                    if (cancelled) return;
                    setForm({
                        name: l.name ?? "",
                        description: l.description ?? "",
                        type: l.type,
                        area: l.area != null ? String(l.area) : "",
                        population:
                            l.population != null ? String(l.population) : "",
                        latitude:
                            l.latitude != null ? String(l.latitude) : "",
                        longitude:
                            l.longitude != null ? String(l.longitude) : "",
                        parentLocationId: l.parentLocationId
                            ? String(l.parentLocationId)
                            : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load location form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    const parentOptions = worldLocations.filter((l) => l.id !== editId);

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
                await updateLocation(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createLocation({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/locations/${targetId}`);
        } catch (err) {
            console.error("Failed to save location:", err);
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
            await deleteLocation(editId);
            navigate("/storymap/locations");
        } catch (err) {
            console.error("Failed to delete location:", err);
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
                glyph="⚑"
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
                    <OrnateField label={t("fields.type")}>
                        <OrnateSelect
                            value={form.type}
                            onChange={(e) =>
                                set("type", e.target.value as LocationType)
                            }
                        >
                            {locationTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`types.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField
                        label={t("fields.parent")}
                        hint={t("form.parentHint")}
                    >
                        <OrnateSelect
                            value={form.parentLocationId}
                            onChange={(e) =>
                                set("parentLocationId", e.target.value)
                            }
                        >
                            <option value="">{t("none")}</option>
                            {parentOptions.map((l) => (
                                <option key={l.id} value={l.id}>
                                    {l.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.area")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="0.01"
                                value={form.area}
                                onChange={(e) => set("area", e.target.value)}
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.population")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                step="1"
                                value={form.population}
                                onChange={(e) =>
                                    set("population", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.latitude")}>
                            <OrnateTextInput
                                type="number"
                                step="0.0001"
                                value={form.latitude}
                                onChange={(e) =>
                                    set("latitude", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.longitude")}>
                            <OrnateTextInput
                                type="number"
                                step="0.0001"
                                value={form.longitude}
                                onChange={(e) =>
                                    set("longitude", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
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
                                ? `/storymap/locations/${editId}`
                                : "/storymap/locations"
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
