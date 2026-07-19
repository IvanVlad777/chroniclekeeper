import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Tag } from "../../ornate";
import { ContentReferenceLinkDto } from "../../../interfaces/loreInterfaces";
import { getReferences } from "../../../api/references";
import { getChapters, getContents, getEpisodes } from "../../../api/contents";
import s from "./referencesSection.module.css";

type EntityType = "Character" | "Location" | "Faction" | "Nation";

interface AppearsInSectionProps {
    worldId: number;
    entityType: EntityType;
    entityId: number;
}

type Appearance = { key: string; route: string; label: string; note: string };

function contentSide(r: ContentReferenceLinkDto): { kind: "contents" | "chapters" | "episodes"; id: number } | null {
    if (r.contentId != null) return { kind: "contents", id: r.contentId };
    if (r.chapterId != null) return { kind: "chapters", id: r.chapterId };
    if (r.episodeId != null) return { kind: "episodes", id: r.episodeId };
    return null;
}

/**
 * Reverse of ReferencesSection: "where does this entity appear" — lists the Content/Chapter/Episode
 * that reference the given Character/Location/Faction/Nation. Read-only (the write side is the
 * ReferencesSection on the content's own detail page).
 */
export function AppearsInSection({ worldId, entityType, entityId }: AppearsInSectionProps) {
    const { t } = useTranslation("content");
    const [items, setItems] = useState<Appearance[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        const filter =
            entityType === "Character"
                ? { characterId: entityId }
                : entityType === "Location"
                ? { locationId: entityId }
                : entityType === "Faction"
                ? { factionId: entityId }
                : { nationId: entityId };
        Promise.all([
            getReferences(filter),
            getContents({ worldId }),
            getChapters({ worldId }),
            getEpisodes({ worldId }),
        ])
            .then(([refs, contents, chapters, episodes]) => {
                if (cancelled) return;
                const names: Record<string, string> = {};
                contents.forEach((c) => (names[`contents:${c.id}`] = c.name));
                chapters.forEach((c) => (names[`chapters:${c.id}`] = c.name));
                episodes.forEach((e) => (names[`episodes:${e.id}`] = e.name));
                const rows: Appearance[] = [];
                refs.forEach((r) => {
                    const side = contentSide(r);
                    if (!side) return;
                    rows.push({
                        key: `${side.kind}:${side.id}:${r.id}`,
                        route: `/storymap/${side.kind}/${side.id}`,
                        label: names[`${side.kind}:${side.id}`] ?? `#${side.id}`,
                        note: r.note,
                    });
                });
                setItems(rows);
            })
            .catch((err) => console.error("Failed to load appearances:", err))
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [worldId, entityType, entityId]);

    return (
        <>
            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("appearsIn")}</span>
                <span className={s.sectionLine} />
            </div>
            <div className={s.chips}>
                {loading ? null : items.length === 0 ? (
                    <p className={s.none}>{t("noAppearances")}</p>
                ) : (
                    items.map((it) => (
                        <span key={it.key} className={s.chipRow} title={it.note}>
                            <Link to={it.route} className={s.chipLink}>
                                <Tag tone="neutral">{it.label}</Tag>
                            </Link>
                        </span>
                    ))
                )}
            </div>
        </>
    );
}
