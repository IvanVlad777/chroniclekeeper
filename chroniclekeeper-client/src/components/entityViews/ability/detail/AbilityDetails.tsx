import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    AbilityDto,
    AbilityLevelDto,
    AbilityRank,
    abilityRanks,
} from "../../../../interfaces/loreInterfaces";
import {
    createAbilityLevel,
    deleteAbilityLevel,
    getAbilityById,
    getAbilityLevels,
    updateAbilityLevel,
} from "../../../../api/abilities";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "✦";

interface AbilityLevelFormState {
    name: string;
    description: string;
    rank: AbilityRank;
}
const emptyLevelForm: AbilityLevelFormState = {
    name: "",
    description: "",
    rank: "Beginner",
};

export default function AbilityDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("ability");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [ability, setAbility] = useState<AbilityDto | null>(null);
    const [levels, setLevels] = useState<AbilityLevelDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    // razina: null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [levelFormFor, setLevelFormFor] = useState<number | null>(null);
    const [levelForm, setLevelForm] =
        useState<AbilityLevelFormState>(emptyLevelForm);
    const [levelError, setLevelError] = useState<string | null>(null);

    useEffect(() => {
        const abilityId = Number(id);
        if (!abilityId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getAbilityById(abilityId)
            .then((data) => {
                if (cancelled) return;
                setAbility(data);
                return getAbilityLevels({ abilityId: data.id });
            })
            .then((lvls) => {
                if (!cancelled && lvls) setLevels(lvls);
            })
            .catch((err) => {
                console.error("Failed to load ability:", err);
                if (cancelled) return;
                if (err?.response?.status === 404) {
                    setNotFound(true);
                } else {
                    setError(t("loaderror"));
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    const openNewLevel = () => {
        setLevelForm(emptyLevelForm);
        setLevelError(null);
        setLevelFormFor(0);
    };
    const openEditLevel = (l: AbilityLevelDto) => {
        setLevelForm({
            name: l.name ?? "",
            description: l.description ?? "",
            rank: (l.rank as AbilityRank) ?? "Beginner",
        });
        setLevelError(null);
        setLevelFormFor(l.id);
    };
    async function onSaveLevel(e: FormEvent) {
        e.preventDefault();
        if (!ability || levelFormFor === null) return;
        if (!levelForm.name.trim()) {
            setLevelError(t("levels.requiredMissing"));
            return;
        }
        setLevelError(null);
        setBusy(true);
        try {
            const payload = {
                name: levelForm.name.trim(),
                description: levelForm.description,
                rank: levelForm.rank,
            };
            if (levelFormFor === 0) {
                await createAbilityLevel({ ...payload, abilityId: ability.id });
            } else {
                await updateAbilityLevel(levelFormFor, payload);
            }
            setLevelFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save ability level:", err);
            setLevelError(apiErrorMessage(err, t("levels.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteLevel(levelId: number, name: string) {
        if (!window.confirm(t("levels.deleteConfirm", { name }))) return;
        setLevelError(null);
        setBusy(true);
        try {
            await deleteAbilityLevel(levelId);
            refetch();
        } catch (err) {
            console.error("Failed to delete ability level:", err);
            setLevelError(apiErrorMessage(err, t("levels.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph={glyph}
                title={t("notfound")}
                action={
                    <Link to="/storymap/abilities" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !ability) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/abilities">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{ability.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{ability.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/abilities/${ability.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("columns.type")}
                        value={ability.type ? t(`types.${ability.type}`) : "—"}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {ability.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{ability.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Ability levels ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("levels.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && levelFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewLevel}>
                        + {t("levels.add")}
                    </button>
                )}
            </div>
            {levels.length === 0 && levelFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                levels.map((l) => (
                    <div key={l.id} className={s.childRow}>
                        <span className={s.childName}>{l.name}</span>
                        <div className={s.childBody}>
                            {l.description && <p className={s.childDesc}>{l.description}</p>}
                            <p className={s.childMeta}>{t(`ranks.${l.rank}`)}</p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditLevel(l)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteLevel(l.id, l.name)}>
                                    {t("levels.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {levelError && levelFormFor === null && (
                <p className={s.miniError} role="alert">{levelError}</p>
            )}
            {levelFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveLevel}>
                    <h3 className={s.childFormTitle}>
                        {levelFormFor === 0 ? t("levels.addTitle") : t("levels.editTitle")}
                    </h3>
                    <OrnateField label={t("levels.name")} required>
                        <OrnateTextInput value={levelForm.name} display maxLength={100} onChange={(e) => setLevelForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("levels.description")}>
                        <OrnateTextArea value={levelForm.description} rows={2} maxLength={4000} onChange={(e) => setLevelForm((f) => ({ ...f, description: e.target.value }))} />
                    </OrnateField>
                    <OrnateField label={t("levels.rank")} required>
                        <OrnateSelect value={levelForm.rank} onChange={(e) => setLevelForm((f) => ({ ...f, rank: e.target.value as AbilityRank }))}>
                            {abilityRanks.map((rank) => (
                                <option key={rank} value={rank}>{t(`ranks.${rank}`)}</option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    {levelError && <p className={s.miniError} role="alert">{levelError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setLevelFormFor(null)}>{t("levels.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("levels.save")}</Button>
                    </div>
                </form>
            )}
        </div>
    );
}
