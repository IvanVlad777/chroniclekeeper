import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, OrnateCheckbox, OrnateField, OrnateSelect, OrnateTextArea, OrnateTextInput } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { createDeity, deleteDeity, getDeityById, updateDeity } from "../../../../api/deities";
import { getReligions } from "../../../../api/religions";
import { getHistories } from "../../../../api/histories";
import { DeityType, DeityUpdateDto, HistoryDto, ReligionDto, deityTypes } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "../mythology.module.css";

interface FormState {
    name: string;
    description: string;
    domain: string;
    worshipMethods: string;
    deityType: DeityType;
    isMonotheistic: boolean;
    isImmortal: boolean;
    canManifestPhysically: boolean;
    grantsPowers: boolean;
    religionId: string;
    historyId: string;
}

const empty: FormState = {
    name: "", description: "", domain: "", worshipMethods: "", deityType: "Unknown",
    isMonotheistic: false, isImmortal: false, canManifestPhysically: false, grantsPowers: false,
    religionId: "", historyId: "",
};

function toDto(f: FormState): DeityUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        domain: f.domain,
        worshipMethods: f.worshipMethods,
        deityType: f.deityType,
        isMonotheistic: f.isMonotheistic,
        isImmortal: f.isImmortal,
        canManifestPhysically: f.canManifestPhysically,
        grantsPowers: f.grantsPowers,
        religionId: f.religionId ? Number(f.religionId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function DeityForm() {
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
        Promise.all([getReligions(selectedWorld.id), getHistories(selectedWorld.id)])
            .then(async ([r, h]) => {
                if (cancelled) return;
                setReligions(r);
                setHistories(h);
                if (isEdit) {
                    const it = await getDeityById(editId);
                    if (cancelled) return;
                    setForm({
                        name: it.name ?? "",
                        description: it.description ?? "",
                        domain: it.domain ?? "",
                        worshipMethods: it.worshipMethods ?? "",
                        deityType: it.deityType,
                        isMonotheistic: it.isMonotheistic,
                        isImmortal: it.isImmortal,
                        canManifestPhysically: it.canManifestPhysically,
                        grantsPowers: it.grantsPowers,
                        religionId: it.religionId ? String(it.religionId) : "",
                        historyId: it.historyId ? String(it.historyId) : "",
                    });
                }
            })
            .catch(() => !cancelled && setLoadError(t("deity.loadError")))
            .finally(() => !cancelled && setLoading(false));
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
                await updateDeity(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createDeity({ ...toDto(form), worldId: selectedWorld.id });
                targetId = created.id;
            }
            navigate(`/storymap/deities/${targetId}`);
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
            await deleteDeity(editId);
            navigate("/storymap/deities");
        } catch (err) {
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) return <LoadingSkeleton variant="block" rows={8} />;
    if (!selectedWorld)
        return <EmptyState glyph="☉" title={t("states.noWorldTitle", { ns: "common" })} text={t("states.noWorldText", { ns: "common" })} />;
    if (loadError) return <ErrorState onRetry={() => setReloadKey((k) => k + 1)} detail={loadError} />;

    return (
        <form className={s.formPage} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>{isEdit ? t("deity.editTitle") : t("deity.newTitle")}</h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("fields.name")} required>
                        <OrnateTextInput value={form.name} maxLength={100} onChange={(e) => set("name", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea value={form.description} rows={5} maxLength={4000} onChange={(e) => set("description", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("deity.fields.domain")}>
                        <OrnateTextInput value={form.domain} maxLength={100} onChange={(e) => set("domain", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("deity.fields.worshipMethods")}>
                        <OrnateTextArea value={form.worshipMethods} rows={3} maxLength={500} onChange={(e) => set("worshipMethods", e.target.value)} />
                    </OrnateField>
                </div>
                <div className={s.col}>
                    <OrnateField label={t("deity.fields.deityType")}>
                        <OrnateSelect value={form.deityType} onChange={(e) => set("deityType", e.target.value as DeityType)}>
                            {deityTypes.map((dt) => <option key={dt} value={dt}>{t(`deityTypes.${dt}`)}</option>)}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.religion")}>
                        <OrnateSelect value={form.religionId} onChange={(e) => set("religionId", e.target.value)}>
                            <option value="">{t("none")}</option>
                            {religions.map((r) => <option key={r.id} value={r.id}>{r.name}</option>)}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateCheckbox label={t("deity.fields.isMonotheistic")} checked={form.isMonotheistic} onChange={(e) => set("isMonotheistic", e.target.checked)} />
                    <OrnateCheckbox label={t("deity.fields.isImmortal")} checked={form.isImmortal} onChange={(e) => set("isImmortal", e.target.checked)} />
                    <OrnateCheckbox label={t("deity.fields.canManifestPhysically")} checked={form.canManifestPhysically} onChange={(e) => set("canManifestPhysically", e.target.checked)} />
                    <OrnateCheckbox label={t("deity.fields.grantsPowers")} checked={form.grantsPowers} onChange={(e) => set("grantsPowers", e.target.checked)} />
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
                <Button variant="ghost" disabled={busy} onClick={() => navigate(isEdit ? `/storymap/deities/${editId}` : "/storymap/deities")}>
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>{busy ? t("form.saving") : t("form.save")}</Button>
            </div>
        </form>
    );
}
