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
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { TagEditor } from "../../../tagging/TagEditor";
import {
    CharacterDto,
    FactionDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addFactionMember,
    getFaction,
    removeFactionMember,
} from "../../../../api/factions";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function FactionDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("faction");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [faction, setFaction] = useState<FactionDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Dodavanje člana
    const [memberOpen, setMemberOpen] = useState(false);
    const [worldCharacters, setWorldCharacters] = useState<
        CharacterDto[] | null
    >(null);
    const [memberPick, setMemberPick] = useState("");
    const [memberRole, setMemberRole] = useState("");
    const [memberSecret, setMemberSecret] = useState(false);
    const [memberError, setMemberError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    useEffect(() => {
        const factionId = Number(id);
        if (!factionId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getFaction(factionId)
            .then((data) => {
                if (!cancelled) setFaction(data);
            })
            .catch((err) => {
                console.error("Failed to load faction:", err);
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

    // Likovi svijeta — lijeno, tek kad se otvori forma za člana
    useEffect(() => {
        if (!memberOpen || worldCharacters !== null || !faction) return;
        let cancelled = false;
        getCharacters(faction.worldId)
            .then((chars) => {
                if (!cancelled) setWorldCharacters(chars);
            })
            .catch((err) =>
                console.error("Failed to load world characters:", err)
            );
        return () => {
            cancelled = true;
        };
    }, [memberOpen, worldCharacters, faction]);

    async function onAddMember(e: FormEvent) {
        e.preventDefault();
        if (!faction || !memberPick) return;
        setMemberError(null);
        setBusy(true);
        try {
            await addFactionMember(faction.id, {
                characterId: Number(memberPick),
                role: memberRole.trim(),
                isSecret: memberSecret,
            });
            setMemberOpen(false);
            setMemberPick("");
            setMemberRole("");
            setMemberSecret(false);
            refetch();
        } catch (err) {
            console.error("Failed to add member:", err);
            setMemberError(apiErrorMessage(err, t("members.addFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onRemoveMember(characterId: number) {
        if (!faction) return;
        setBusy(true);
        try {
            await removeFactionMember(faction.id, characterId);
            refetch();
        } catch (err) {
            console.error("Failed to remove member:", err);
            setMemberError(apiErrorMessage(err, t("members.removeFailed")));
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph="⚔"
                title={t("notfound")}
                action={
                    <Link to="/storymap/factions" className={s.refLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !faction) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const kicker = [
        t(`types.${faction.type}`),
        faction.isSecretive ? t("secretive") : null,
    ]
        .filter(Boolean)
        .join(" · ");
    const memberCandidates = (worldCharacters ?? []).filter(
        (c) => !faction.members.some((m) => m.characterId === c.id)
    );

    return (
        <div className={s.page}>
            <div className={s.header}>
                <div className={s.breadcrumb}>
                    <Link to="/storymap/factions">{t("listTitle")}</Link>
                    <span className={s.breadcrumbSep}>/</span>
                    <span className={s.breadcrumbCurrent}>{faction.name}</span>
                </div>
                <div className={s.headerRow}>
                    <div className={s.emblem} aria-hidden="true">
                        ⚔
                    </div>
                    <div className={s.headerMain}>
                        <div className={s.kicker}>{kicker}</div>
                        <h1 className={s.name}>{faction.name}</h1>
                        {faction.motto && (
                            <div className={s.motto}>“{faction.motto}”</div>
                        )}
                    </div>
                    {canEdit && (
                        <Button
                            variant="ghost"
                            onClick={() =>
                                navigate(
                                    `/storymap/factions/${faction.id}/edit`
                                )
                            }
                        >
                            {t("form.edit")}
                        </Button>
                    )}
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={4}>
                    <OrnateDisplayBox
                        label={t("fields.type")}
                        value={t(`types.${faction.type}`)}
                    />
                    <OrnateDisplayBox
                        label={t("fields.headquarters")}
                        value={
                            faction.headquarters ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/locations/${faction.headquarters.id}`}
                                >
                                    {faction.headquarters.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.leader")}
                        value={
                            faction.leader ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/characters/${faction.leader.id}`}
                                >
                                    {faction.leader.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.isSecretive")}
                        value={
                            faction.isSecretive
                                ? t("secretive")
                                : t("none")
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.body}>
                <div>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>
                            {t("fields.description")}
                        </span>
                        <span className={s.sectionLine} />
                        <span className={s.sectionStar}>✦</span>
                    </div>
                    {faction.description ? (
                        <p className={`${s.prose} ${s.dropCap}`}>
                            {faction.description}
                        </p>
                    ) : (
                        <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
                    )}
                </div>

                <div className={s.side}>
                    <div>
                        <div className={s.listLabel}>
                            {t("members.label")}{" "}
                            <span className={s.memberCount}>
                                {faction.members.length}
                            </span>
                            {canEdit && !memberOpen && (
                                <button
                                    type="button"
                                    className={s.addInline}
                                    onClick={() => setMemberOpen(true)}
                                >
                                    + {t("members.add")}
                                </button>
                            )}
                        </div>
                        {faction.members.length === 0 && !memberOpen ? (
                            <p className={s.none}>{t("none")}</p>
                        ) : (
                            faction.members.map((m) => (
                                <div key={m.id} className={s.listRow}>
                                    <span className={s.listThumb} />
                                    <Link
                                        to={`/storymap/characters/${m.characterId}`}
                                        className={s.memberName}
                                    >
                                        {m.characterName}
                                    </Link>
                                    {m.isSecret && (
                                        <span
                                            className={s.secretMark}
                                            title={t("members.secret")}
                                        >
                                            ◐
                                        </span>
                                    )}
                                    {m.role && (
                                        <span className={s.memberRole}>
                                            {m.role}
                                        </span>
                                    )}
                                    {canEdit && (
                                        <button
                                            type="button"
                                            className={s.chipRemove}
                                            aria-label={t("members.remove")}
                                            disabled={busy}
                                            onClick={() =>
                                                onRemoveMember(m.characterId)
                                            }
                                        >
                                            ×
                                        </button>
                                    )}
                                </div>
                            ))
                        )}
                        {memberOpen && (
                            <form
                                className={s.miniForm}
                                onSubmit={onAddMember}
                            >
                                <OrnateField
                                    label={t("members.character")}
                                    required
                                >
                                    <OrnateSelect
                                        value={memberPick}
                                        onChange={(e) =>
                                            setMemberPick(e.target.value)
                                        }
                                    >
                                        <option value="">{t("none")}</option>
                                        {memberCandidates.map((c) => (
                                            <option key={c.id} value={c.id}>
                                                {c.name}
                                            </option>
                                        ))}
                                    </OrnateSelect>
                                </OrnateField>
                                <OrnateField label={t("members.role")}>
                                    <OrnateTextInput
                                        value={memberRole}
                                        maxLength={100}
                                        onChange={(e) =>
                                            setMemberRole(e.target.value)
                                        }
                                    />
                                </OrnateField>
                                <OrnateCheckbox
                                    label={t("members.secret")}
                                    checked={memberSecret}
                                    onChange={(e) =>
                                        setMemberSecret(e.target.checked)
                                    }
                                />
                                {memberError && (
                                    <p className={s.miniError} role="alert">
                                        {memberError}
                                    </p>
                                )}
                                <div className={s.miniActions}>
                                    <Button
                                        variant="ghost"
                                        size="sm"
                                        disabled={busy}
                                        onClick={() => {
                                            setMemberOpen(false);
                                            setMemberError(null);
                                        }}
                                    >
                                        {t("form.cancel")}
                                    </Button>
                                    <Button
                                        type="submit"
                                        size="sm"
                                        disabled={busy || !memberPick}
                                    >
                                        {t("members.addConfirm")}
                                    </Button>
                                </div>
                            </form>
                        )}
                        {!memberOpen && memberError && (
                            <p className={s.miniError} role="alert">
                                {memberError}
                            </p>
                        )}
                    </div>
                    <TagEditor
                        worldId={faction.worldId}
                        targetType="Faction"
                        targetId={faction.id}
                        tags={faction.tags}
                        canEdit={canEdit}
                        onChanged={refetch}
                        showLabel
                    />
                </div>
            </div>
        </div>
    );
}
