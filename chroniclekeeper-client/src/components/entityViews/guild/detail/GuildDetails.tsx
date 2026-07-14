import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateCheckbox,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import {
    FactionDto,
    GuildDetailsDto,
    GuildRankLevel,
    ProfessionDto,
    SocialClassDto,
    guildRankLevels,
} from "../../../../interfaces/loreInterfaces";
import {
    addGuildFaction,
    addGuildProfession,
    addGuildSocialClass,
    createGuildRank,
    deleteGuildRank,
    getGuildById,
    removeGuildFaction,
    removeGuildProfession,
    removeGuildSocialClass,
    updateGuildRank,
} from "../../../../api/guilds";
import { getFactions } from "../../../../api/factions";
import { getProfessions } from "../../../../api/professions";
import { getSocialClasses } from "../../../../api/socialClasses";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚒";

interface GuildRankFormState {
    name: string;
    description: string;
    rankTitle: string;
    rankLevel: GuildRankLevel;
    hasLeadershipAuthority: boolean;
}
const emptyGuildRankForm: GuildRankFormState = {
    name: "",
    description: "",
    rankTitle: "",
    rankLevel: "Apprentice",
    hasLeadershipAuthority: false,
};

export default function GuildDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("guild");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [guild, setGuild] = useState<GuildDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    const [factionCandidates, setFactionCandidates] = useState<
        FactionDto[] | null
    >(null);
    const [professionCandidates, setProfessionCandidates] = useState<
        ProfessionDto[] | null
    >(null);
    const [socialClassCandidates, setSocialClassCandidates] = useState<
        SocialClassDto[] | null
    >(null);

    // Rang: null = zatvoreno, 0 = novo, >0 = edit tog id-a
    const [rankFormFor, setRankFormFor] = useState<number | null>(null);
    const [rankForm, setRankForm] = useState<GuildRankFormState>(
        emptyGuildRankForm
    );
    const [rankError, setRankError] = useState<string | null>(null);

    useEffect(() => {
        const guildId = Number(id);
        if (!guildId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getGuildById(guildId)
            .then((data) => {
                if (!cancelled) setGuild(data);
            })
            .catch((err) => {
                console.error("Failed to load guild:", err);
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

    // ----- Guild ranks -----

    const openNewRank = () => {
        setRankForm(emptyGuildRankForm);
        setRankError(null);
        setRankFormFor(0);
    };
    const openEditRank = (r: GuildDetailsDto["guildRanks"][number]) => {
        setRankForm({
            name: r.name ?? "",
            description: r.description ?? "",
            rankTitle: r.rankTitle ?? "",
            rankLevel: r.rankLevel,
            hasLeadershipAuthority: r.hasLeadershipAuthority,
        });
        setRankError(null);
        setRankFormFor(r.id);
    };
    async function onSaveRank(e: FormEvent) {
        e.preventDefault();
        if (!guild || rankFormFor === null) return;
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
                rankLevel: rankForm.rankLevel,
                hasLeadershipAuthority: rankForm.hasLeadershipAuthority,
            };
            if (rankFormFor === 0) {
                await createGuildRank({ ...payload, guildId: guild.id });
            } else {
                await updateGuildRank(rankFormFor, payload);
            }
            setRankFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save guild rank:", err);
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
            await deleteGuildRank(rankId);
            refetch();
        } catch (err) {
            console.error("Failed to delete guild rank:", err);
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
                    <Link to="/storymap/guilds" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !guild) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const systemLink = (
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
                <Link to="/storymap/guilds">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{guild.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {[guild.guildType, guild.primaryActivity]
                            .filter(Boolean)
                            .join(" · ") || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{guild.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/guilds/${guild.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.isGovernmentSanctioned")}
                        value={guild.isGovernmentSanctioned ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.taxationSystem")}
                        value={systemLink(guild.taxationSystem, "taxation-systems")}
                    />
                    <OrnateDisplayBox
                        label={t("fields.industry")}
                        value={systemLink(guild.industry, "industries")}
                    />
                    <OrnateDisplayBox
                        label={t("fields.legalSystem")}
                        value={systemLink(guild.legalSystem, "legal-systems")}
                    />
                    <OrnateDisplayBox
                        label={t("fields.educationSystem")}
                        value={systemLink(
                            guild.educationSystem,
                            "education-systems"
                        )}
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={systemLink(guild.history, "histories")}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {guild.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>{guild.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Guild ranks ----- */}
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
            {guild.guildRanks.length === 0 && rankFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                guild.guildRanks.map((r) => (
                    <div key={r.id} className={s.childRow}>
                        <span className={s.childName}>{r.name}</span>
                        <div className={s.childBody}>
                            {r.description && (
                                <p className={s.childDesc}>{r.description}</p>
                            )}
                            <p className={s.childMeta}>
                                {[
                                    r.rankTitle,
                                    t(`rankLevels.${r.rankLevel}`),
                                    r.hasLeadershipAuthority
                                        ? t("ranks.leadership")
                                        : null,
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
                                onChange={(e) =>
                                    setRankForm((f) => ({
                                        ...f,
                                        rankTitle: e.target.value,
                                    }))
                                }
                            />
                        </OrnateField>
                        <OrnateField label={t("ranks.rankLevel")}>
                            <OrnateSelect
                                value={rankForm.rankLevel}
                                onChange={(e) =>
                                    setRankForm((f) => ({
                                        ...f,
                                        rankLevel: e.target
                                            .value as GuildRankLevel,
                                    }))
                                }
                            >
                                {guildRankLevels.map((level) => (
                                    <option key={level} value={level}>
                                        {t(`rankLevels.${level}`)}
                                    </option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <OrnateCheckbox
                        label={t("ranks.hasLeadershipAuthority")}
                        checked={rankForm.hasLeadershipAuthority}
                        onChange={(e) =>
                            setRankForm((f) => ({
                                ...f,
                                hasLeadershipAuthority: e.target.checked,
                            }))
                        }
                    />
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

            {/* ----- Cross-links ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.factions")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={guild.factions}
                candidates={factionCandidates}
                onLoadCandidates={() =>
                    getFactions(guild.worldId).then(setFactionCandidates)
                }
                onAdd={(factionId) => addGuildFaction(guild.id, factionId)}
                onRemove={(factionId) => removeGuildFaction(guild.id, factionId)}
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(factionId) => `/storymap/factions/${factionId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.professions")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={guild.memberProfessions}
                candidates={professionCandidates}
                onLoadCandidates={() =>
                    getProfessions(guild.worldId).then(setProfessionCandidates)
                }
                onAdd={(professionId) =>
                    addGuildProfession(guild.id, professionId)
                }
                onRemove={(professionId) =>
                    removeGuildProfession(guild.id, professionId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(professionId) => `/storymap/professions/${professionId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.socialClasses")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={guild.socialClasses}
                candidates={socialClassCandidates}
                onLoadCandidates={() =>
                    getSocialClasses(guild.worldId).then(setSocialClassCandidates)
                }
                onAdd={(socialClassId) =>
                    addGuildSocialClass(guild.id, socialClassId)
                }
                onRemove={(socialClassId) =>
                    removeGuildSocialClass(guild.id, socialClassId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(socialClassId) =>
                    `/storymap/social-classes/${socialClassId}`
                }
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />
        </div>
    );
}
