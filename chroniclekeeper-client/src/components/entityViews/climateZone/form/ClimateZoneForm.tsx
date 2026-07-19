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
    createClimateZone,
    deleteClimateZone,
    getClimateZoneById,
    updateClimateZone,
} from "../../../../api/climateZones";
import { getHistories } from "../../../../api/histories";
import {
    ClimateZoneType,
    ClimateZoneUpdateDto,
    climateZoneTypes,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    zoneType: ClimateZoneType;
    averageTemperature: string;
    averageHumidity: string;
    averagePrecipitation: string;
    hasDistinctSeasons: boolean;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    zoneType: "Temperate",
    averageTemperature: "0",
    averageHumidity: "0",
    averagePrecipitation: "0",
    hasDistinctSeasons: false,
    historyId: "",
};

function toDto(f: FormState): ClimateZoneUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        zoneType: f.zoneType,
        averageTemperature: Number(f.averageTemperature) || 0,
        averageHumidity: Number(f.averageHumidity) || 0,
        averagePrecipitation: Number(f.averagePrecipitation) || 0,
        hasDistinctSeasons: f.hasDistinctSeasons,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /climate-zones/new i /climate-zones/:id/edit. */
export default function ClimateZoneForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("climateZone");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
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

        getHistories(selectedWorld.id)
            .then(async (historiesData) => {
                if (cancelled) return;
                setHistoryOptions(
                    historiesData.map((h) => ({ value: h.id, label: h.name }))
                );

                if (isEdit) {
                    const z = await getClimateZoneById(editId);
                    if (cancelled) return;
                    setForm({
                        name: z.name ?? "",
                        description: z.description ?? "",
                        zoneType: z.zoneType,
                        averageTemperature: String(z.averageTemperature ?? 0),
                        averageHumidity: String(z.averageHumidity ?? 0),
                        averagePrecipitation: String(z.averagePrecipitation ?? 0),
                        hasDistinctSeasons: z.hasDistinctSeasons,
                        historyId: z.historyId ? String(z.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load climate zone form data:", err);
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
                await updateClimateZone(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createClimateZone({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/climate-zones/${targetId}`);
        } catch (err) {
            console.error("Failed to save climate zone:", err);
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
            await deleteClimateZone(editId);
            navigate("/storymap/climate-zones");
        } catch (err) {
            console.error("Failed to delete climate zone:", err);
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
                glyph="☁"
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
                            onChange={(e) => set("description", e.target.value)}
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

                <div className={s.col}>
                    <OrnateField label={t("fields.zoneType")}>
                        <OrnateSelect
                            value={form.zoneType}
                            onChange={(e) =>
                                set("zoneType", e.target.value as ClimateZoneType)
                            }
                        >
                            {climateZoneTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`zoneTypes.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.averageTemperature")}>
                            <OrnateTextInput
                                type="number"
                                value={form.averageTemperature}
                                onChange={(e) =>
                                    set("averageTemperature", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.averageHumidity")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                max={100}
                                value={form.averageHumidity}
                                onChange={(e) =>
                                    set("averageHumidity", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("fields.averagePrecipitation")}>
                        <OrnateTextInput
                            type="number"
                            min={0}
                            value={form.averagePrecipitation}
                            onChange={(e) =>
                                set("averagePrecipitation", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.hasDistinctSeasons")}
                        checked={form.hasDistinctSeasons}
                        onChange={(e) =>
                            set("hasDistinctSeasons", e.target.checked)
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
                                ? `/storymap/climate-zones/${editId}`
                                : "/storymap/climate-zones"
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
