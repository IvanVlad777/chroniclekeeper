import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextInput,
    StatusPill,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { TagEditor } from "../../../tagging/TagEditor";
import {
    CharacterDetailsDto,
    CharacterDto,
    RelationshipType,
    relationshipTypes,
} from "../../../../interfaces/loreInterfaces";
import {
    addRelationship,
    getCharacter,
    getCharacters,
    removeRelationship,
} from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

export default function CharacterDetail() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("character");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [character, setCharacter] = useState<CharacterDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Likovi svijeta za select nove veze (učitava se samo za editore)
    const [worldCharacters, setWorldCharacters] = useState<CharacterDto[]>([]);

    // Inline forma za novu vezu
    const [relOpen, setRelOpen] = useState(false);
    const [relTarget, setRelTarget] = useState("");
    const [relType, setRelType] = useState<RelationshipType>("Friend");
    const [relDescription, setRelDescription] = useState("");
    const [relSecret, setRelSecret] = useState(false);
    const [relError, setRelError] = useState<string | null>(null);

    const [busy, setBusy] = useState(false);

    useEffect(() => {
        const characterId = Number(id);
        if (!characterId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getCharacter(characterId)
            .then((data) => {
                if (!cancelled) setCharacter(data);
            })
            .catch((err) => {
                console.error("Failed to load character:", err);
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

    // Editorima trebaju likovi svijeta za add-formu veze
    useEffect(() => {
        if (!canEdit || !character) return;
        let cancelled = false;
        getCharacters(character.worldId)
            .then((chars) => {
                if (!cancelled) setWorldCharacters(chars);
            })
            .catch((err) =>
                console.error("Failed to load world characters:", err)
            );
        return () => {
            cancelled = true;
        };
    }, [canEdit, character?.id, character?.worldId, reloadKey]); // eslint-disable-line react-hooks/exhaustive-deps

    async function onAddRelationship(e: FormEvent) {
        e.preventDefault();
        if (!character || !relTarget) return;
        setRelError(null);
        setBusy(true);
        try {
            await addRelationship(character.id, {
                relatedCharacterId: Number(relTarget),
                type: relType,
                description: relDescription.trim(),
                isSecret: relSecret,
            });
            setRelOpen(false);
            setRelTarget("");
            setRelType("Friend");
            setRelDescription("");
            setRelSecret(false);
            refetch();
        } catch (err) {
            console.error("Failed to add relationship:", err);
            setRelError(apiErrorMessage(err, t("rel.addFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onRemoveRelationship(relationshipId: number) {
        if (!character) return;
        setBusy(true);
        try {
            await removeRelationship(character.id, relationshipId);
            refetch();
        } catch (err) {
            console.error("Failed to remove relationship:", err);
            setRelError(apiErrorMessage(err, t("rel.removeFailed")));
            setBusy(false);
        }
    }

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph="♟"
                title={t("notfound")}
                action={
                    <Link to="/storymap/characters" className={s.relLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !character) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const fmtDate = (d?: string | null) =>
        d ? new Date(d).toLocaleDateString() : null;
    const lifespan =
        [fmtDate(character.birthDate), fmtDate(character.deathDate)]
            .filter(Boolean)
            .join(" — ") || dash;
    const kicker = [character.species?.name, character.race?.name]
        .filter(Boolean)
        .join(" · ");
    const status = character.deathDate ? "dead" : "living";

    const parentLink = (ref?: { id: number; name: string } | null) =>
        ref ? (
            <Link to={`/storymap/characters/${ref.id}`} className={s.relLink}>
                {ref.name}
            </Link>
        ) : (
            dash
        );

    const relCandidates = worldCharacters.filter(
        (c) => c.id !== character.id
    );

    return (
        <div className={s.page}>
            <div className={s.header}>
                <div className={s.breadcrumb}>
                    <Link to="/storymap/characters">{t("listTitle")}</Link>
                    <span className={s.breadcrumbSep}>/</span>
                    <span className={s.breadcrumbCurrent}>
                        {character.name}
                    </span>
                </div>
                <div className={s.headerRow}>
                    <div className={s.headerMain}>
                        {kicker && <div className={s.kicker}>{kicker}</div>}
                        <h1 className={s.name}>{character.name}</h1>
                        {character.title && (
                            <div className={s.epithet}>{character.title}</div>
                        )}
                    </div>
                    <div className={s.headerActions}>
                        <StatusPill status={status}>
                            {t(`status.${status}`)}
                        </StatusPill>
                        {canEdit && (
                            <Button
                                variant="ghost"
                                onClick={() =>
                                    navigate(
                                        `/storymap/characters/${character.id}/edit`
                                    )
                                }
                            >
                                {t("form.edit")}
                            </Button>
                        )}
                    </div>
                </div>
                <div className={s.flourish}>
                    <span className={s.flourishStar}>✦</span>
                    <span className={s.flourishLine} />
                </div>
            </div>

            <div className={s.body}>
                <div className={s.side}>
                    <div className={s.portrait} aria-hidden="true">
                        {character.name?.charAt(0)}
                    </div>
                    <div className={s.facts}>
                        <OrnateDisplayBox
                            label={t("lifespan")}
                            value={lifespan}
                        />
                        <OrnateDisplayBox
                            label={t("haircolor")}
                            value={character.hairColor || dash}
                        />
                        <OrnateDisplayBox
                            label={t("eyecolor")}
                            value={character.eyeColor || dash}
                        />
                        <OrnateDisplayBox
                            label={t("height")}
                            value={
                                character.height != null
                                    ? String(character.height)
                                    : dash
                            }
                        />
                        <OrnateDisplayBox
                            label={t("weight")}
                            value={
                                character.weight != null
                                    ? String(character.weight)
                                    : dash
                            }
                        />
                        <OrnateDisplayBox
                            label={t("father")}
                            value={parentLink(character.father)}
                        />
                        <OrnateDisplayBox
                            label={t("mother")}
                            value={parentLink(character.mother)}
                        />
                    </div>

                    <TagEditor
                        worldId={character.worldId}
                        targetType="Character"
                        targetId={character.id}
                        tags={character.tags}
                        canEdit={canEdit}
                        onChanged={refetch}
                    />
                </div>

                <div>
                    <div className={s.sectionHead}>
                        <span className={s.sectionTitle}>
                            {t("description")}
                        </span>
                        <span className={s.sectionLine} />
                        <span className={s.sectionStar}>✦</span>
                    </div>
                    {character.description ? (
                        <p className={`${s.prose} ${s.dropCap}`}>
                            {character.description}
                        </p>
                    ) : (
                        <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
                    )}

                    <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                        <span className={s.sectionTitle}>{t("features")}</span>
                        <span className={s.sectionLine} />
                    </div>
                    {character.specialPhysicalFeatures ? (
                        <p className={s.prose}>
                            {character.specialPhysicalFeatures}
                        </p>
                    ) : (
                        <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
                    )}

                    <div className={s.linkGrid}>
                        <div>
                            <div className={s.listLabel}>{t("factions")}</div>
                            {character.factions.length === 0 ? (
                                <p className={s.none}>{t("none")}</p>
                            ) : (
                                character.factions.map((f) => (
                                    <div key={f.id} className={s.listRow}>
                                        <span className={s.listThumb} />
                                        <span className={s.listName}>
                                            {f.name}
                                        </span>
                                    </div>
                                ))
                            )}
                        </div>
                        <div>
                            <div className={s.listLabel}>
                                {t("relationships")}
                                {canEdit && !relOpen && (
                                    <button
                                        type="button"
                                        className={s.addInline}
                                        onClick={() => setRelOpen(true)}
                                    >
                                        + {t("rel.add")}
                                    </button>
                                )}
                            </div>
                            {character.relationships.length === 0 &&
                            !relOpen ? (
                                <p className={s.none}>{t("none")}</p>
                            ) : (
                                character.relationships.map((r) => (
                                    <div key={r.id} className={s.listRow}>
                                        <span className={s.relType}>
                                            {t(`relTypes.${r.type}`, {
                                                defaultValue: r.type,
                                            })}
                                        </span>
                                        <Link
                                            to={`/storymap/characters/${r.relatedCharacterId}`}
                                            className={s.relLink}
                                        >
                                            {r.relatedCharacterName}
                                        </Link>
                                        {canEdit && (
                                            <button
                                                type="button"
                                                className={s.chipRemove}
                                                aria-label={t("rel.remove")}
                                                disabled={busy}
                                                onClick={() =>
                                                    onRemoveRelationship(r.id)
                                                }
                                            >
                                                ×
                                            </button>
                                        )}
                                    </div>
                                ))
                            )}
                            {relOpen && (
                                <form
                                    className={s.miniForm}
                                    onSubmit={onAddRelationship}
                                >
                                    <OrnateField
                                        label={t("rel.target")}
                                        required
                                    >
                                        <OrnateSelect
                                            value={relTarget}
                                            onChange={(e) =>
                                                setRelTarget(e.target.value)
                                            }
                                        >
                                            <option value="">
                                                {t("none")}
                                            </option>
                                            {relCandidates.map((c) => (
                                                <option
                                                    key={c.id}
                                                    value={c.id}
                                                >
                                                    {c.name}
                                                </option>
                                            ))}
                                        </OrnateSelect>
                                    </OrnateField>
                                    <OrnateField label={t("rel.type")}>
                                        <OrnateSelect
                                            value={relType}
                                            onChange={(e) =>
                                                setRelType(
                                                    e.target
                                                        .value as RelationshipType
                                                )
                                            }
                                        >
                                            {relationshipTypes.map((rt) => (
                                                <option key={rt} value={rt}>
                                                    {t(`relTypes.${rt}`)}
                                                </option>
                                            ))}
                                        </OrnateSelect>
                                    </OrnateField>
                                    <OrnateField
                                        label={t("rel.description")}
                                    >
                                        <OrnateTextInput
                                            value={relDescription}
                                            maxLength={500}
                                            onChange={(e) =>
                                                setRelDescription(
                                                    e.target.value
                                                )
                                            }
                                        />
                                    </OrnateField>
                                    <OrnateCheckbox
                                        label={t("rel.secret")}
                                        checked={relSecret}
                                        onChange={(e) =>
                                            setRelSecret(e.target.checked)
                                        }
                                    />
                                    {relError && (
                                        <p
                                            className={s.miniError}
                                            role="alert"
                                        >
                                            {relError}
                                        </p>
                                    )}
                                    <div className={s.miniActions}>
                                        <Button
                                            variant="ghost"
                                            size="sm"
                                            disabled={busy}
                                            onClick={() => {
                                                setRelOpen(false);
                                                setRelError(null);
                                            }}
                                        >
                                            {t("form.cancel")}
                                        </Button>
                                        <Button
                                            type="submit"
                                            size="sm"
                                            disabled={busy || !relTarget}
                                        >
                                            {t("rel.addConfirm")}
                                        </Button>
                                    </div>
                                </form>
                            )}
                            {!relOpen && relError && (
                                <p className={s.miniError} role="alert">
                                    {relError}
                                </p>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
