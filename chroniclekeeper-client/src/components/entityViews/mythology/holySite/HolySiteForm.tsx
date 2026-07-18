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
    createHolySite,
    deleteHolySite,
    getHolySiteById,
    updateHolySite,
} from "../../../../api/holySites";
import { getReligions } from "../../../../api/religions";
import { getLocations } from "../../../../api/locations";
import { getDeities } from "../../../../api/deities";
import { getHistories } from "../../../../api/histories";
import {
    DeityDto,
    HistoryDto,
    HolySiteUpdateDto,
    LocationDto,
    ReligionDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "../mythology.module.css";

interface FormState {
    name: string;
    description: string;
    significance: string;
    isPilgrimageDestination: boolean;
    religionId: string;
    locationId: string;
    deityId: string;
    historyId: string;
}

const empty: FormState = {
    name: "",
    description: "",
    significance: "",
    isPilgrimageDestination: false,
    religionId: "",
    locationId: "",
    deityId: "",
    historyId: "",
};

function toDto(f: FormState): HolySiteUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        significance: f.significance,
        isPilgrimageDestination: f.isPilgrimageDestination,
        religionId: Number(f.religionId),
        locationId: Number(f.locationId),
        deityId: f.deityId ? Number(f.deityId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function HolySiteForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("mythology");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(empty);
    const [religions, setReligions] = useState<ReligionDto[]>([]);
    const [locations, setLocations] = useState<LocationDto[]>([]);
    const [deities, setDeities] = useState<DeityDto[]>([]);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(k: K, v: FormState[K]) =>
        setForm((f) => ({ ...f, [k]: v }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;
        let cancelled = false;
        setLoading(true);
        setLoadError(null);
        Promise.all([
            getReligions(selectedWorld.id),
            getLocations(selectedWorld.id),
            getDeities(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(async ([r, l, d, h]) => {
                if (cancelled) return;
                setReligions(r);
                setLocations(l);
                setDeities(d);
                setHistories(h);
                if (isEdit) {
                    const it = await getHolySiteById(editId);
                    if (cancelled) return;
                    setForm({
                        name: it.name ?? "",
                        description: it.description ?? "",
                        significance: it.significance ?? "",
                        isPilgrimageDestination: it.isPilgrimageDestination,
                        religionId: String(it.religionId ?? ""),
                        locationId: String(it.locationId ?? ""),
                        deityId: it.deityId ? String(it.deityId) : "",
                        historyId: it.historyId ? String(it.historyId) : "",
                    });
                }
            })
            .catch(() => !cancelled && setLoadError(t("holySite.loadError")))
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim() || !form.religionId || !form.locationId) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateHolySite(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createHolySite({ ...toDto(form), worldId: selectedWorld.id });
                targetId = created.id;
            }
            navigate(`/storymap/holy-sites/${targetId}`);
        } catch (err) {
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) return;
        setBusy(true);
        try {
            await deleteHolySite(editId);
            navigate("/storymap/holy-sites");
        } catch (err) {
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) return <LoadingSkeleton variant="block" rows={8} />;
    if (!selectedWorld)
        return (
            <EmptyState
                glyph="⛩"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    if (loadError) return <ErrorState onRetry={() => setReloadKey((k) => k + 1)} detail={loadError} />;

    return (
        <form className={s.formPage} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>{isEdit ? t("holySite.editTitle") : t("holySite.newTitle")}</h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("fields.name")} required>
                        <OrnateTextInput value={form.name} maxLength={100} onChange={(e) => set("name", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea value={form.description} rows={6} maxLength={4000} onChange={(e) => set("description", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("holySite.fields.significance")}>
                        <OrnateTextArea value={form.significance} rows={3} maxLength={500} onChange={(e) => set("significance", e.target.value)} />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("holySite.fields.isPilgrimageDestination")}
                        checked={form.isPilgrimageDestination}
                        onChange={(e) => set("isPilgrimageDestination", e.target.checked)}
                    />
                </div>

                <div className={s.col}>
                    <OrnateField label={t("fields.religion")} required>
                        <OrnateSelect value={form.religionId} onChange={(e) => set("religionId", e.target.value)}>
                            <option value="">{t("form.select")}</option>
                            {religions.map((r) => (
                                <option key={r.id} value={r.id}>{r.name}</option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.location")} required>
                        <OrnateSelect value={form.locationId} onChange={(e) => set("locationId", e.target.value)}>
                            <option value="">{t("form.select")}</option>
                            {locations.map((l) => (
                                <option key={l.id} value={l.id}>{l.name}</option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.deity")}>
                        <OrnateSelect value={form.deityId} onChange={(e) => set("deityId", e.target.value)}>
                            <option value="">{t("none")}</option>
                            {deities.map((d) => (
                                <option key={d.id} value={d.id}>{d.name}</option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("form.history")}>
                        <OrnateSelect value={form.historyId} onChange={(e) => set("historyId", e.target.value)}>
                            <option value="">{t("none")}</option>
                            {histories.map((h) => (
                                <option key={h.id} value={h.id}>{h.name}</option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                </div>
            </div>

            {saveError && <p className={s.formError} role="alert">{saveError}</p>}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button variant="danger" disabled={busy} onClick={onDelete}>{t("form.delete")}</Button>
                )}
                <span className={s.footerSpacer} />
                <Button variant="ghost" disabled={busy} onClick={() => navigate(isEdit ? `/storymap/holy-sites/${editId}` : "/storymap/holy-sites")}>
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>{busy ? t("form.saving") : t("form.save")}</Button>
            </div>
        </form>
    );
}
