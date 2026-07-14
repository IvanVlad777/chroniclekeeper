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
    createGuild,
    deleteGuild,
    getGuildById,
    updateGuild,
} from "../../../../api/guilds";
import { getTaxationSystems } from "../../../../api/taxationSystems";
import { getIndustries } from "../../../../api/industries";
import { getLegalSystems } from "../../../../api/legalSystems";
import { getEducationSystems } from "../../../../api/educationSystems";
import { getHistories } from "../../../../api/histories";
import {
    EducationSystemDto,
    GuildUpdateDto,
    HistoryDto,
    IndustryDto,
    LegalSystemDto,
    TaxationSystemDto,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    guildType: string;
    primaryActivity: string;
    isGovernmentSanctioned: boolean;
    taxationSystemId: string;
    industryId: string;
    legalSystemId: string;
    educationSystemId: string;
    historyId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    guildType: "",
    primaryActivity: "",
    isGovernmentSanctioned: false,
    taxationSystemId: "",
    industryId: "",
    legalSystemId: "",
    educationSystemId: "",
    historyId: "",
};

function toDto(f: FormState): GuildUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        guildType: f.guildType,
        primaryActivity: f.primaryActivity,
        isGovernmentSanctioned: f.isGovernmentSanctioned,
        taxationSystemId: f.taxationSystemId ? Number(f.taxationSystemId) : null,
        industryId: f.industryId ? Number(f.industryId) : null,
        legalSystemId: f.legalSystemId ? Number(f.legalSystemId) : null,
        educationSystemId: f.educationSystemId
            ? Number(f.educationSystemId)
            : null,
        historyId: f.historyId ? Number(f.historyId) : null,
    };
}

/** Zajednička forma za /guilds/new i /guilds/:id/edit. */
export default function GuildForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("guild");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [taxationSystems, setTaxationSystems] = useState<TaxationSystemDto[]>(
        []
    );
    const [industries, setIndustries] = useState<IndustryDto[]>([]);
    const [legalSystems, setLegalSystems] = useState<LegalSystemDto[]>([]);
    const [educationSystems, setEducationSystems] = useState<
        EducationSystemDto[]
    >([]);
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

        Promise.all([
            getTaxationSystems(selectedWorld.id),
            getIndustries(selectedWorld.id),
            getLegalSystems(selectedWorld.id),
            getEducationSystems(selectedWorld.id),
            getHistories(selectedWorld.id),
        ])
            .then(
                async ([
                    taxationData,
                    industriesData,
                    legalData,
                    educationData,
                    historiesData,
                ]) => {
                    if (cancelled) return;
                    setTaxationSystems(taxationData);
                    setIndustries(industriesData);
                    setLegalSystems(legalData);
                    setEducationSystems(educationData);
                    setHistories(historiesData);

                    if (isEdit) {
                        const g = await getGuildById(editId);
                        if (cancelled) return;
                        setForm({
                            name: g.name ?? "",
                            description: g.description ?? "",
                            guildType: g.guildType ?? "",
                            primaryActivity: g.primaryActivity ?? "",
                            isGovernmentSanctioned: g.isGovernmentSanctioned,
                            taxationSystemId: g.taxationSystemId
                                ? String(g.taxationSystemId)
                                : "",
                            industryId: g.industryId ? String(g.industryId) : "",
                            legalSystemId: g.legalSystemId
                                ? String(g.legalSystemId)
                                : "",
                            educationSystemId: g.educationSystemId
                                ? String(g.educationSystemId)
                                : "",
                            historyId: g.historyId ? String(g.historyId) : "",
                        });
                    }
                }
            )
            .catch((err) => {
                console.error("Failed to load guild form data:", err);
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
                await updateGuild(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createGuild({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/guilds/${targetId}`);
        } catch (err) {
            console.error("Failed to save guild:", err);
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
            await deleteGuild(editId);
            navigate("/storymap/guilds");
        } catch (err) {
            console.error("Failed to delete guild:", err);
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
                glyph="⚒"
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
                        <OrnateField label={t("fields.guildType")}>
                            <OrnateTextInput
                                value={form.guildType}
                                maxLength={100}
                                placeholder={t("fields.guildTypeHint")}
                                onChange={(e) =>
                                    set("guildType", e.target.value)
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("fields.primaryActivity")}>
                            <OrnateTextInput
                                value={form.primaryActivity}
                                maxLength={200}
                                placeholder={t("fields.primaryActivityHint")}
                                onChange={(e) =>
                                    set("primaryActivity", e.target.value)
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("fields.isGovernmentSanctioned")}
                        checked={form.isGovernmentSanctioned}
                        onChange={(e) =>
                            set("isGovernmentSanctioned", e.target.checked)
                        }
                    />
                </div>

                <div className={s.col}>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.taxationSystem")}>
                            <OrnateSelect
                                value={form.taxationSystemId}
                                onChange={(e) =>
                                    set("taxationSystemId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {taxationSystems.map((ts) => (
                                    <option key={ts.id} value={ts.id}>
                                        {ts.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("fields.industry")}>
                            <OrnateSelect
                                value={form.industryId}
                                onChange={(e) =>
                                    set("industryId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {industries.map((i) => (
                                    <option key={i.id} value={i.id}>
                                        {i.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("fields.legalSystem")}>
                            <OrnateSelect
                                value={form.legalSystemId}
                                onChange={(e) =>
                                    set("legalSystemId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {legalSystems.map((l) => (
                                    <option key={l.id} value={l.id}>
                                        {l.name}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("fields.educationSystem")}>
                            <OrnateSelect
                                value={form.educationSystemId}
                                onChange={(e) =>
                                    set("educationSystemId", e.target.value)
                                }
                            >
                                <option value="">{t("none")}</option>
                                {educationSystems.map((es) => (
                                    <option key={es.id} value={es.id}>
                                        {es.name}
                                    </option>
                                ))}
                            </OrnateSelect>
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
                                ? `/storymap/guilds/${editId}`
                                : "/storymap/guilds"
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
