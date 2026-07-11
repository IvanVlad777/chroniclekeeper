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
    createClimateDetail,
    deleteClimateDetail,
    getClimateDetailById,
    updateClimateDetail,
} from "../../../../api/climateDetails";
import { getHistories } from "../../../../api/histories";
import {
    ClimateDetailUpdateDto,
    HistoryDto,
    NotableWeatherPhenomena,
    WindDirection,
    notableWeatherPhenomena,
    windDirections,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    averageTemperature: string;
    humidity: string;
    precipitation: string;
    windSpeed: string;
    windDirection: WindDirection;
    isExtremeClimate: boolean;
    notableWeatherPhenomena: NotableWeatherPhenomena;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    averageTemperature: "0",
    humidity: "0",
    precipitation: "0",
    windSpeed: "0",
    windDirection: "Variable",
    isExtremeClimate: false,
    notableWeatherPhenomena: "None",
    historyId: "",
};

function toDto(f: FormState): ClimateDetailUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        averageTemperature: Number(f.averageTemperature) || 0,
        humidity: Number(f.humidity) || 0,
        precipitation: Number(f.precipitation) || 0,
        windSpeed: Number(f.windSpeed) || 0,
        windDirection: f.windDirection,
        isExtremeClimate: f.isExtremeClimate,
        notableWeatherPhenomena: f.notableWeatherPhenomena,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /climate-details/new i /climate-details/:id/edit. */
export default function ClimateDetailForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("climateDetail");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
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
                setHistories(historiesData);

                if (isEdit) {
                    const d = await getClimateDetailById(editId);
                    if (cancelled) return;
                    setForm({
                        name: d.name ?? "",
                        description: d.description ?? "",
                        averageTemperature: String(d.averageTemperature ?? 0),
                        humidity: String(d.humidity ?? 0),
                        precipitation: String(d.precipitation ?? 0),
                        windSpeed: String(d.windSpeed ?? 0),
                        windDirection: d.windDirection,
                        isExtremeClimate: d.isExtremeClimate,
                        notableWeatherPhenomena: d.notableWeatherPhenomena,
                        historyId: d.historyId ? String(d.historyId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load climate detail form data:", err);
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
                await updateClimateDetail(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createClimateDetail({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/climate-details/${targetId}`);
        } catch (err) {
            console.error("Failed to save climate detail:", err);
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
            await deleteClimateDetail(editId);
            navigate("/storymap/climate-details");
        } catch (err) {
            console.error("Failed to delete climate detail:", err);
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
                glyph="☂"
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
                        <OrnateField label={t("fields.humidity")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                max={100}
                                value={form.humidity}
                                onChange={(e) => set("humidity", e.target.value)}
                            />
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.precipitation")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.precipitation}
                                onChange={(e) =>
                                    set("precipitation", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.windSpeed")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={form.windSpeed}
                                onChange={(e) => set("windSpeed", e.target.value)}
                            />
                        </OrnateField>
                    </div>
                    <OrnateField label={t("form.history")}>
                        <OrnateSelect
                            value={form.historyId}
                            onChange={(e) => set("historyId", e.target.value)}
                        >
                            <option value="">{t("none")}</option>
                            {histories.map((h) => (
                                <option key={h.id} value={h.id}>
                                    {h.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.windDirection")}>
                        <OrnateSelect
                            value={form.windDirection}
                            onChange={(e) =>
                                set("windDirection", e.target.value as WindDirection)
                            }
                        >
                            {windDirections.map((dir) => (
                                <option key={dir} value={dir}>
                                    {t(`windDirections.${dir}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.notableWeatherPhenomena")}>
                        <OrnateSelect
                            value={form.notableWeatherPhenomena}
                            onChange={(e) =>
                                set(
                                    "notableWeatherPhenomena",
                                    e.target.value as NotableWeatherPhenomena
                                )
                            }
                        >
                            {notableWeatherPhenomena.map((phenomenon) => (
                                <option key={phenomenon} value={phenomenon}>
                                    {t(`notableWeatherPhenomena.${phenomenon}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isExtremeClimate")}
                        checked={form.isExtremeClimate}
                        onChange={(e) =>
                            set("isExtremeClimate", e.target.checked)
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
                                ? `/storymap/climate-details/${editId}`
                                : "/storymap/climate-details"
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
