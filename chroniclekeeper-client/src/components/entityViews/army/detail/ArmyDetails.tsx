import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateCheckbox,
    OrnateDisplayBox,
    OrnateField,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import {
    ArmyDetailsDto,
    BattleDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addArmyBattle,
    getArmyById,
    removeArmyBattle,
} from "../../../../api/armies";
import {
    createMilitaryUnit,
    deleteMilitaryUnit,
    getMilitaryUnits,
} from "../../../../api/militaryUnits";
import { getBattles } from "../../../../api/battles";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import { apiErrorMessage } from "../../../../utils/apiError";
import { MilitaryUnitDto } from "../../../../interfaces/loreInterfaces";
import s from "./styles.module.css";

const glyph = "⚔";

interface UnitFormState {
    name: string;
    unitType: string;
    size: string;
    isElite: boolean;
}
const emptyUnitForm: UnitFormState = {
    name: "",
    unitType: "",
    size: "0",
    isElite: false,
};

export default function ArmyDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("army");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [army, setArmy] = useState<ArmyDetailsDto | null>(null);
    const [units, setUnits] = useState<MilitaryUnitDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [battleCandidates, setBattleCandidates] = useState<BattleDto[] | null>(
        null
    );

    const [unitFormOpen, setUnitFormOpen] = useState(false);
    const [unitForm, setUnitForm] = useState<UnitFormState>(emptyUnitForm);
    const [unitError, setUnitError] = useState<string | null>(null);

    useEffect(() => {
        const armyId = Number(id);
        if (!armyId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getArmyById(armyId)
            .then((data) => {
                if (cancelled) return;
                setArmy(data);
                return getMilitaryUnits({ armyId: data.id });
            })
            .then((unitData) => {
                if (!cancelled && unitData) setUnits(unitData);
            })
            .catch((err) => {
                console.error("Failed to load army:", err);
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

    async function onCreateUnit(e: FormEvent) {
        e.preventDefault();
        if (!army) return;
        if (!unitForm.name.trim()) {
            setUnitError(t("units.requiredMissing"));
            return;
        }
        setUnitError(null);
        setBusy(true);
        try {
            await createMilitaryUnit({
                name: unitForm.name.trim(),
                description: "",
                unitType: unitForm.unitType,
                size: Number(unitForm.size) || 0,
                isElite: unitForm.isElite,
                historyId: null,
                belongsToArmyId: army.id,
            });
            setUnitForm(emptyUnitForm);
            setUnitFormOpen(false);
            refetch();
        } catch (err) {
            console.error("Failed to create unit:", err);
            setUnitError(apiErrorMessage(err, t("units.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDeleteUnit(unitId: number, name: string) {
        if (!window.confirm(t("units.deleteConfirm", { name }))) return;
        setUnitError(null);
        setBusy(true);
        try {
            await deleteMilitaryUnit(unitId);
            refetch();
        } catch (err) {
            console.error("Failed to delete unit:", err);
            setUnitError(apiErrorMessage(err, t("units.deleteFailed")));
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
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !army) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const refLink = (
        ref: { id: number; name: string } | null | undefined,
        path: string
    ) =>
        ref ? (
            <Link className={s.refLink} to={`/storymap/${path}/${ref.id}`}>
                {ref.name}
            </Link>
        ) : (
            dash
        );

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/armies">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{army.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {army.isStandingArmy
                            ? t("standing.yes")
                            : t("standing.no")}
                    </div>
                    <h1 className={s.name}>{army.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/armies/${army.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.size")}
                        value={army.size}
                    />
                    <OrnateDisplayBox
                        label={t("fields.militaryOrganization")}
                        value={refLink(
                            army.militaryOrganization,
                            "military-organizations"
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("fields.faction")}
                        value={refLink(army.faction, "factions")}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {army.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{army.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Units ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("units.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && !unitFormOpen && (
                    <button
                        type="button"
                        className={s.addInline}
                        onClick={() => {
                            setUnitForm(emptyUnitForm);
                            setUnitError(null);
                            setUnitFormOpen(true);
                        }}
                    >
                        + {t("units.add")}
                    </button>
                )}
            </div>
            {units.length === 0 && !unitFormOpen ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                units.map((u) => (
                    <div key={u.id} className={s.childRow}>
                        <Link
                            className={s.childName}
                            to={`/storymap/military-units/${u.id}`}
                        >
                            {u.name}
                        </Link>
                        <div className={s.childBody}>
                            <p className={s.childMeta}>
                                {[
                                    u.unitType,
                                    `${u.size}`,
                                    u.isElite ? t("units.elite") : null,
                                ]
                                    .filter(Boolean)
                                    .join(" · ")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <Link
                                    className={s.childActionBtn}
                                    to={`/storymap/military-units/${u.id}`}
                                >
                                    {t("units.open")}
                                </Link>
                                <button
                                    type="button"
                                    className={`${s.childActionBtn} ${s.childActionDanger}`}
                                    disabled={busy}
                                    onClick={() => onDeleteUnit(u.id, u.name)}
                                >
                                    {t("units.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {unitError && !unitFormOpen && (
                <p className={s.miniError} role="alert">
                    {unitError}
                </p>
            )}
            {unitFormOpen && (
                <form className={s.childForm} onSubmit={onCreateUnit}>
                    <h3 className={s.childFormTitle}>{t("units.addTitle")}</h3>
                    <OrnateField label={t("units.name")} required>
                        <OrnateTextInput
                            value={unitForm.name}
                            display
                            maxLength={100}
                            onChange={(e) =>
                                setUnitForm((f) => ({
                                    ...f,
                                    name: e.target.value,
                                }))
                            }
                        />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("units.unitType")}>
                            <OrnateTextInput
                                value={unitForm.unitType}
                                maxLength={100}
                                placeholder={t("units.unitTypeHint")}
                                onChange={(e) =>
                                    setUnitForm((f) => ({
                                        ...f,
                                        unitType: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("units.size")}>
                            <OrnateTextInput
                                type="number"
                                min={0}
                                value={unitForm.size}
                                onChange={(e) =>
                                    setUnitForm((f) => ({
                                        ...f,
                                        size: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("units.isElite")}
                        checked={unitForm.isElite}
                        onChange={(e) =>
                            setUnitForm((f) => ({
                                ...f,
                                isElite: e.target.checked,
                            }))
                        }
                    />
                    {unitError && (
                        <p className={s.miniError} role="alert">
                            {unitError}
                        </p>
                    )}
                    <div className={s.miniActions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => setUnitFormOpen(false)}
                        >
                            {t("units.cancel")}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy}>
                            {t("units.save")}
                        </Button>
                    </div>
                </form>
            )}

            {/* ----- Battles ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("battles.label")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={army.battles}
                candidates={battleCandidates}
                onLoadCandidates={() =>
                    getBattles(army.worldId).then(setBattleCandidates)
                }
                onAdd={(battleId) => addArmyBattle(army.id, battleId)}
                onRemove={(battleId) => removeArmyBattle(army.id, battleId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(battleId) => `/storymap/battles/${battleId}`}
                addLabel={t("battles.add")}
                noneLabel={t("none")}
                pickLabel={t("battles.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("battles.confirm")}
                removeLabel={(name) => t("battles.remove", { name })}
                addFailedLabel={t("battles.addFailed")}
                removeFailedLabel={t("battles.removeFailed")}
            />

            <HistoryBlock
                targetType="Army"
                targetId={army.id}
                worldId={army.worldId}
                history={army.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
