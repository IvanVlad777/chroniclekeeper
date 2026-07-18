import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, OrnateCheckbox, OrnateField, OrnateSelect, OrnateTextArea, OrnateTextInput } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createReligiousOrder,
    deleteReligiousOrder,
    getReligiousOrderById,
    updateReligiousOrder,
} from "../../../../api/religiousOrders";
import { getReligions } from "../../../../api/religions";
import { getHistories } from "../../../../api/histories";
import { HistoryDto, ReligionDto, ReligiousOrderUpdateDto } from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "../mythology.module.css";

interface FormState {
    name: string;
    description: string;
    orderType: string;
    beliefs: string;
    isMilitant: boolean;
    isSecretive: boolean;
    religionId: string;
    historyId: string;
}

const empty: FormState = { name: "", description: "", orderType: "", beliefs: "", isMilitant: false, isSecretive: false, religionId: "", historyId: "" };

function toDto(f: FormState): ReligiousOrderUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        orderType: f.orderType,
        beliefs: f.beliefs,
        isMilitant: f.isMilitant,
        isSecretive: f.isSecretive,
        religionId: f.religionId ? Number(f.religionId) : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

export default function ReligiousOrderForm() {
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
                    const it = await getReligiousOrderById(editId);
                    if (cancelled) return;
                    setForm({
                        name: it.name ?? "",
                        description: it.description ?? "",
                        orderType: it.orderType ?? "",
                        beliefs: it.beliefs ?? "",
                        isMilitant: it.isMilitant,
                        isSecretive: it.isSecretive,
                        religionId: it.religionId ? String(it.religionId) : "",
                        historyId: it.historyId ? String(it.historyId) : "",
                    });
                }
            })
            .catch(() => !cancelled && setLoadError(t("religiousOrder.loadError")))
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
                await updateReligiousOrder(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createReligiousOrder({ ...toDto(form), worldId: selectedWorld.id });
                targetId = created.id;
            }
            navigate(`/storymap/religious-orders/${targetId}`);
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
            await deleteReligiousOrder(editId);
            navigate("/storymap/religious-orders");
        } catch (err) {
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || (loading && selectedWorld)) return <LoadingSkeleton variant="block" rows={8} />;
    if (!selectedWorld)
        return <EmptyState glyph="🕍" title={t("states.noWorldTitle", { ns: "common" })} text={t("states.noWorldText", { ns: "common" })} />;
    if (loadError) return <ErrorState onRetry={() => setReloadKey((k) => k + 1)} detail={loadError} />;

    return (
        <form className={s.formPage} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>{isEdit ? t("religiousOrder.editTitle") : t("religiousOrder.newTitle")}</h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("fields.name")} required>
                        <OrnateTextInput value={form.name} maxLength={100} onChange={(e) => set("name", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea value={form.description} rows={5} maxLength={4000} onChange={(e) => set("description", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("religiousOrder.fields.beliefs")}>
                        <OrnateTextArea value={form.beliefs} rows={3} maxLength={500} onChange={(e) => set("beliefs", e.target.value)} />
                    </OrnateField>
                </div>
                <div className={s.col}>
                    <OrnateField label={t("religiousOrder.fields.orderType")}>
                        <OrnateTextInput value={form.orderType} maxLength={100} onChange={(e) => set("orderType", e.target.value)} />
                    </OrnateField>
                    <OrnateField label={t("fields.religion")}>
                        <OrnateSelect value={form.religionId} onChange={(e) => set("religionId", e.target.value)}>
                            <option value="">{t("none")}</option>
                            {religions.map((r) => <option key={r.id} value={r.id}>{r.name}</option>)}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateCheckbox label={t("religiousOrder.fields.isMilitant")} checked={form.isMilitant} onChange={(e) => set("isMilitant", e.target.checked)} />
                    <OrnateCheckbox label={t("religiousOrder.fields.isSecretive")} checked={form.isSecretive} onChange={(e) => set("isSecretive", e.target.checked)} />
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
                <Button variant="ghost" disabled={busy} onClick={() => navigate(isEdit ? `/storymap/religious-orders/${editId}` : "/storymap/religious-orders")}>
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>{busy ? t("form.saving") : t("form.save")}</Button>
            </div>
        </form>
    );
}
