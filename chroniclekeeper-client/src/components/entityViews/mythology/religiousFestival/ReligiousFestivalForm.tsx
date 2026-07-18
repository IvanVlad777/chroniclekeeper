import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, OrnateCheckbox, OrnateField, OrnateSelect, OrnateTextArea, OrnateTextInput } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createReligiousFestival,
    deleteReligiousFestival,
    getReligiousFestivalById,
    updateReligiousFestival,
} from "../../../../api/religiousFestivals";
import { getReligions } from "../../../../api/religions";
import { getHolySites } from "../../../../api/holySites";
import { getHistories } from "../../../../api/histories";
import { HistoryDto, HolySiteDto, ReligionDto, ReligiousFestivalUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "../mythology.module.css";

interface FormState {
    name: string;
    description: string;
    startDate: string;
    durationDays: string;
    traditions: string;
    isPilgrimageEvent: boolean;
    religionId: string;
    holySiteId: string;
    historyId: string;
}

const empty: FormState = {
    name: "", description: "", startDate: "", durationDays: "0", traditions: "",
    isPilgrimageEvent: false, religionId: "", holySiteId: "", historyId: "",
};

function toDto(f: FormState): ReligiousFestivalUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        startDate: f.startDate || null,
        durationDays: Number(f.durationDays) || 0,
        traditions: f.traditions,
        isPilgrimageEvent: f.isPilgrimageEvent,
        religionId: Number(f.religionId),
        holySiteId: f.holySiteId ? Number(f.holySiteId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function ReligiousFestivalForm() {
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
    const [holySites, setHolySites] = useState<HolySiteDto[]>([]);
    const [histories, setHistories] = useState<HistoryDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(k: K, v: FormState[K]) => setForm((f) => ({ ...f, [k]: v }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;
        let cancelled = false;
        setLoading(true);
        setLoadError(null);
        Promise.all([getReligions(selectedWorld.id), getHolySites(selectedWorld.id), getHistories(selectedWorld.id)])
            .then(async ([r, hs, h]) => {
                if (cancelled) return;
                setReligions(r);
                setHolySites(hs);
                setHistories(h);
                if (isEdit) {
                    const it = await getReligiousFestivalById(editId);
                    if (cancelled) return;
                    setForm({
                        name: it.name ?? "",
                        description: it.description ?? "",
                        startDate: it.startDate ?? "",
                        durationDays: String(it.durationDays ?? 0),
                        traditions: it.traditions ?? "",
                        isPilgrimageEvent: it.isPilgrimageEvent,
                        religionId: String(it.religionId ?? ""),
                        holySiteId: it.holySiteId ? String(it.holySiteId) : "",
                        historyId: it.historyId ? String(it.historyId) : "",
                    });
                }
            })
            .catch(() => !cancelled && setLoadError(t("religiousFestival.loadError")))
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim() || !form.religionId) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateReligiousFestival(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createReligiousFestival({ ...toDto(form), worldId: selectedWorld.id });
                targetId = created.id;
            }
            navigate(`/storymap/religious-festivals/${targetId}`);
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
            await deleteReligiousFestival(editId);
            navigate("/storymap/religious-festivals");
        } catch (err) {
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) return <LoadingSkeleton variant="block" rows={8} />;
    if (!selectedWorld)
        return <EmptyState glyph="🎆" title={t("states.noWorldTitle", { ns: "common" })} text={t("states.noWorldText", { ns: "common" })} />;
    if (loadError) return <ErrorState onRetry={() => setReloadKey((k) => k + 1)} detail={loadError} />;

    return (
        <form className={s.formPage} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>{isEdit ? t("religiousFestival.editTitle") : t("religiousFestival.newTitle")}</h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("fields.name")} required>
                        <OrnateTextInput value={form.name} maxLength={100} onChange={(e) => set("name", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea value={form.description} rows={5} maxLength={4000} onChange={(e) => set("description", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("religiousFestival.fields.traditions")}>
                        <OrnateTextArea value={form.traditions} rows={3} maxLength={500} onChange={(e) => set("traditions", e.target.value)} />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("religiousFestival.fields.isPilgrimageEvent")}
                        checked={form.isPilgrimageEvent}
                        onChange={(e) => set("isPilgrimageEvent", e.target.checked)}
                    />
                </div>
                <div className={s.col}>
                    <OrnateField label={t("religiousFestival.fields.startDate")}>
                        <OrnateTextInput value={form.startDate} maxLength={100} placeholder={t("religiousFestival.fields.startDateHint")} onChange={(e) => set("startDate", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("religiousFestival.fields.durationDays")}>
                        <OrnateTextInput type="number" value={form.durationDays} onChange={(e) => set("durationDays", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("fields.religion")} required>
                        <OrnateSelect value={form.religionId} onChange={(e) => set("religionId", e.target.value)}>
                            <option value="">{t("form.select")}</option>
                            {religions.map((r) => <option key={r.id} value={r.id}>{r.name}</option>)}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.holySite")}>
                        <OrnateSelect value={form.holySiteId} onChange={(e) => set("holySiteId", e.target.value)}>
                            <option value="">{t("none")}</option>
                            {holySites.map((h) => <option key={h.id} value={h.id}>{h.name}</option>)}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("form.history")}>
                        <OrnateSelect value={form.historyId} onChange={(e) => set("historyId", e.target.value)}>
                            <option value="">{t("none")}</option>
                            {histories.map((h) => <option key={h.id} value={h.id}>{h.name}</option>)}
                        </OrnateSelect>
                    </OrnateField>
                </div>
            </div>

            {saveError && <p className={s.formError} role="alert">{saveError}</p>}

            <div className={s.footer}>
                {isEdit && canDelete && <Button variant="danger" disabled={busy} onClick={onDelete}>{t("form.delete")}</Button>}
                <span className={s.footerSpacer} />
                <Button variant="ghost" disabled={busy} onClick={() => navigate(isEdit ? `/storymap/religious-festivals/${editId}` : "/storymap/religious-festivals")}>
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>{busy ? t("form.saving") : t("form.save")}</Button>
            </div>
        </form>
    );
}
