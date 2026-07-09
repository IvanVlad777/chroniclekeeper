import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { OrnateDisplayBox, StatusPill, Tag } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { CharacterDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getCharacter } from "../../../../api/characters";
import s from "./styles.module.css";

// TODO: Edit gumb dolazi kad klijent dobije mutacijske endpointe (create/update).
export default function CharacterDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("character");

    const [character, setCharacter] = useState<CharacterDetailsDto | null>(
        null
    );
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

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

    const facts: { label: string; value: string }[] = [
        { label: t("lifespan"), value: lifespan },
        { label: t("haircolor"), value: character.hairColor || dash },
        { label: t("eyecolor"), value: character.eyeColor || dash },
        {
            label: t("height"),
            value: character.height != null ? String(character.height) : dash,
        },
        {
            label: t("weight"),
            value: character.weight != null ? String(character.weight) : dash,
        },
        { label: t("father"), value: character.father?.name ?? dash },
        { label: t("mother"), value: character.mother?.name ?? dash },
    ];

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
                        {facts.map((f) => (
                            <OrnateDisplayBox
                                key={f.label}
                                label={f.label}
                                value={f.value}
                            />
                        ))}
                    </div>
                    {character.tags.length > 0 && (
                        <div className={s.tags}>
                            {character.tags.map((tag) => (
                                <Tag key={tag.id}>{tag.name}</Tag>
                            ))}
                        </div>
                    )}
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
                            </div>
                            {character.relationships.length === 0 ? (
                                <p className={s.none}>{t("none")}</p>
                            ) : (
                                character.relationships.map((r) => (
                                    <div key={r.id} className={s.listRow}>
                                        <span className={s.relType}>
                                            {r.type}
                                        </span>
                                        <Link
                                            to={`/storymap/characters/${r.relatedCharacterId}`}
                                            className={s.relLink}
                                        >
                                            {r.relatedCharacterName}
                                        </Link>
                                    </div>
                                ))
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
