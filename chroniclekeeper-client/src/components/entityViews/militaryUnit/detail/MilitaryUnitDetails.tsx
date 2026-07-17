import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import {
    MilitaryEquipmentDto,
    MilitaryRankDto,
    MilitaryUnitDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addUnitEquipment,
    getMilitaryUnitById,
    removeUnitEquipment,
} from "../../../../api/militaryUnits";
import {
    createMilitaryRank,
    deleteMilitaryRank,
    getMilitaryRanks,
    updateMilitaryRank,
} from "../../../../api/militaryRanks";
import { getMilitaryEquipment } from "../../../../api/militaryEquipment";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const glyph = "🎖";

interface RankFormState {
    name: string;
    description: string;
    rankTitle: string;
    rankLevel: string;
}
const emptyRankForm: RankFormState = {
    name: "",
    description: "",
    rankTitle: "",
    rankLevel: "0",
};

export default function MilitaryUnitDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("militaryUnit");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [unit, setUnit] = useState<MilitaryUnitDetailsDto | null>(null);
    const [ranks, setRanks] = useState<MilitaryRankDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [equipmentCandidates, setEquipmentCandidates] = useState<
        MilitaryEquipmentDto[] | null
    >(null);

    // Rank inline: null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [rankFormFor, setRankFormFor] = useState<number | null>(null);
    const [rankForm, setRankForm] = useState<RankFormState>(emptyRankForm);
    const [rankError, setRankError] = useState<string | null>(null);

    useEffect(() => {
        const unitId = Number(id);
        if (!unitId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getMilitaryUnitById(unitId)
            .then((data) => {
                if (cancelled) return;
                setUnit(data);
                return getMilitaryRanks({ unitId: data.id });
            })
            .then((rankData) => {
                if (!cancelled && rankData) setRanks(rankData);
            })
            .catch((err) => {
                console.error("Failed to load unit:", err);
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    const openNewRank = () => {
        setRankForm(emptyRankForm);
        setRankError(null);
        setRankFormFor(0);
    };
    const openEditRank = (r: MilitaryRankDto) => {
        setRankForm({
            name: r.name ?? "",
            description: r.description ?? "",
            rankTitle: r.rankTitle ?? "",
            rankLevel: String(r.rankLevel ?? 0),
        });
        setRankError(null);
        setRankFormFor(r.id);
    };
    async function onSaveRank(e: FormEvent) {
        e.preventDefault();
        if (!unit || rankFormFor === null) return;
        if (!rankForm.name.trim()) {
            setRankError(t("ranks.requiredMissing"));
            return;
        }
        setRankError(null);
        setBusy(true);
        try {
            const payload = {
                name: rankForm.name.trim(),
                description: rankForm.description,
                rankTitle: rankForm.rankTitle,
                rankLevel: Number(rankForm.rankLevel) || 0,
                historyId: null,
            };
            if (rankFormFor === 0) {
                await createMilitaryRank({
                    ...payload,
                    militaryUnitId: unit.id,
                });
            } else {
                await updateMilitaryRank(rankFormFor, payload);
            }
            setRankFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save rank:", err);
            setRankError(apiErrorMessage(err, t("ranks.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteRank(rankId: number, name: string) {
        if (!window.confirm(t("ranks.deleteConfirm", { name }))) return;
        setRankError(null);
        setBusy(true);
        try {
            await deleteMilitaryRank(rankId);
            refetch();
        } catch (err) {
            console.error("Failed to delete rank:", err);
            setRankError(apiErrorMessage(err, t("ranks.deleteFailed")));
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
                    <Link to="/storymap/armies" className={s.backLink}>
                        ← {t("backToArmies")}
                    </Link>
                }
            />
        );
    }
    if (error || !unit) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const armyLink = `/storymap/armies/${unit.belongsToArmyId}`;

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/armies">{t("backToArmies")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                {unit.belongsToArmy ? (
                    <Link to={armyLink}>{unit.belongsToArmy.name}</Link>
                ) : (
                    <span>{t("fields.army")}</span>
                )}
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{unit.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {unit.unitType || t("title")}
                    </div>
                    <h1 className={s.name}>{unit.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/military-units/${unit.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("fields.unitType")}
                        value={unit.unitType || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.size")}
                        value={unit.size}
                    />
                    <OrnateDisplayBox
                        label={t("fields.elite")}
                        value={unit.isElite ? t("fields.elite") : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.army")}
                        value={
                            unit.belongsToArmy ? (
                                <Link className={s.refLink} to={armyLink}>
                                    {unit.belongsToArmy.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {unit.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{unit.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Ranks ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("ranks.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && rankFormFor === null && (
                    <button
                        type="button"
                        className={s.addInline}
                        onClick={openNewRank}
                    >
                        + {t("ranks.add")}
                    </button>
                )}
            </div>
            {ranks.length === 0 && rankFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                ranks.map((r) => (
                    <div key={r.id} className={s.childRow}>
                        <span className={s.childName}>{r.name}</span>
                        <div className={s.childBody}>
                            {r.description && (
                                <p className={s.childDesc}>{r.description}</p>
                            )}
                            <p className={s.childMeta}>
                                {[
                                    r.rankTitle,
                                    `${t("ranks.rankLevel")}: ${r.rankLevel}`,
                                ]
                                    .filter(Boolean)
                                    .join(" · ")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button
                                    type="button"
                                    className={s.childActionBtn}
                                    disabled={busy}
                                    onClick={() => openEditRank(r)}
                                >
                                    {t("form.edit")}
                                </button>
                                <button
                                    type="button"
                                    className={`${s.childActionBtn} ${s.childActionDanger}`}
                                    disabled={busy}
                                    onClick={() => onDeleteRank(r.id, r.name)}
                                >
                                    {t("ranks.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {rankError && rankFormFor === null && (
                <p className={s.miniError} role="alert">
                    {rankError}
                </p>
            )}
            {rankFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveRank}>
                    <h3 className={s.childFormTitle}>
                        {rankFormFor === 0
                            ? t("ranks.addTitle")
                            : t("ranks.editTitle")}
                    </h3>
                    <OrnateField label={t("ranks.name")} required>
                        <OrnateTextInput
                            value={rankForm.name}
                            display
                            maxLength={100}
                            onChange={(e) =>
                                setRankForm((f) => ({
                                    ...f,
                                    name: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("ranks.description")}>
                        <OrnateTextArea
                            value={rankForm.description}
                            rows={2}
                            maxLength={4000}
                            onChange={(e) =>
                                setRankForm((f) => ({
                                    ...f,
                                    description: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("ranks.rankTitle")}>
                            <OrnateTextInput
                                value={rankForm.rankTitle}
                                maxLength={100}
                                placeholder={t("ranks.rankTitleHint")}
                                onChange={(e) =>
                                    setRankForm((f) => ({
                                        ...f,
                                        rankTitle: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("ranks.rankLevel")}>
                            <OrnateTextInput
                                type="number"
                                value={rankForm.rankLevel}
                                onChange={(e) =>
                                    setRankForm((f) => ({
                                        ...f,
                                        rankLevel: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                    </div>
                    {rankError && (
                        <p className={s.miniError} role="alert">
                            {rankError}
                        </p>
                    )}
                    <div className={s.miniActions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => setRankFormFor(null)}
                        >
                            {t("ranks.cancel")}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy}>
                            {t("ranks.save")}
                        </Button>
                    </div>
                </form>
            )}

            {/* ----- Equipment ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("equipment.label")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={unit.equipment}
                candidates={equipmentCandidates}
                onLoadCandidates={() =>
                    getMilitaryEquipment(unit.worldId).then(
                        setEquipmentCandidates
                    )
                }
                onAdd={(equipmentId) => addUnitEquipment(unit.id, equipmentId)}
                onRemove={(equipmentId) =>
                    removeUnitEquipment(unit.id, equipmentId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(equipmentId) =>
                    `/storymap/military-equipment/${equipmentId}`
                }
                addLabel={t("equipment.add")}
                noneLabel={t("none")}
                pickLabel={t("equipment.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("equipment.confirm")}
                removeLabel={(name) => t("equipment.remove", { name })}
                addFailedLabel={t("equipment.addFailed")}
                removeFailedLabel={t("equipment.removeFailed")}
            />

            <HistoryBlock
                targetType="MilitaryUnit"
                targetId={unit.id}
                worldId={unit.worldId}
                history={unit.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
