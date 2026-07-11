import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, OrnateField, OrnateSelect, OrnateTextInput, Tag } from "../../ornate";
import {
    ContentReferenceLinkDto,
    ReferenceDto,
} from "../../../interfaces/loreInterfaces";
import {
    createReference,
    deleteReference,
    getReferences,
} from "../../../api/references";
import { getCharacters } from "../../../api/characters";
import { getLocations } from "../../../api/locations";
import { getFactions } from "../../../api/factions";
import { getNations } from "../../../api/nations";
import { apiErrorMessage } from "../../../utils/apiError";
import s from "./referencesSection.module.css";

type EntityType = "Character" | "Location" | "Faction" | "Nation";
const entityTypes: EntityType[] = ["Character", "Location", "Faction", "Nation"];

const entityRoutes: Record<EntityType, string> = {
    Character: "characters",
    Location: "locations",
    Faction: "factions",
    Nation: "nations",
};

interface ReferencesSectionProps {
    worldId: number;
    /** Točno jedan od ova tri je postavljen — koji "sadržajni komad" prikazuje ovu sekciju. */
    contentId?: number;
    chapterId?: number;
    episodeId?: number;
    canEdit: boolean;
}

function entityTypeAndId(r: ContentReferenceLinkDto): { type: EntityType; id: number } | null {
    if (r.characterId != null) return { type: "Character", id: r.characterId };
    if (r.locationId != null) return { type: "Location", id: r.locationId };
    if (r.factionId != null) return { type: "Faction", id: r.factionId };
    if (r.nationId != null) return { type: "Nation", id: r.nationId };
    return null;
}

/** "Tko/što se ovdje pojavljuje" — dijeljena sekcija za Content/Chapter/Episode detalje. */
export function ReferencesSection({
    worldId,
    contentId,
    chapterId,
    episodeId,
    canEdit,
}: ReferencesSectionProps) {
    const { t } = useTranslation("content");
    const [references, setReferences] = useState<ContentReferenceLinkDto[]>([]);
    const [names, setNames] = useState<Record<string, string>>({});
    const [loading, setLoading] = useState(true);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = () => setReloadKey((k) => k + 1);

    const [open, setOpen] = useState(false);
    const [entityType, setEntityType] = useState<EntityType>("Character");
    const [candidates, setCandidates] = useState<ReferenceDto[] | null>(null);
    const [pick, setPick] = useState("");
    const [note, setNote] = useState("");
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        Promise.all([
            getReferences({ contentId, chapterId, episodeId }),
            getCharacters(worldId),
            getLocations(worldId),
            getFactions(worldId),
            getNations(worldId),
        ])
            .then(([refs, chars, locs, facs, nats]) => {
                if (cancelled) return;
                setReferences(refs);
                const map: Record<string, string> = {};
                chars.forEach((c) => (map[`Character:${c.id}`] = c.name));
                locs.forEach((l) => (map[`Location:${l.id}`] = l.name));
                facs.forEach((f) => (map[`Faction:${f.id}`] = f.name));
                nats.forEach((n) => (map[`Nation:${n.id}`] = n.name));
                setNames(map);
            })
            .catch((err) => console.error("Failed to load references:", err))
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [worldId, contentId, chapterId, episodeId, reloadKey]);

    const loadCandidates = async (type: EntityType) => {
        setCandidates(null);
        setPick("");
        let data: ReferenceDto[];
        switch (type) {
            case "Character":
                data = await getCharacters(worldId);
                break;
            case "Location":
                data = await getLocations(worldId);
                break;
            case "Faction":
                data = await getFactions(worldId);
                break;
            case "Nation":
                data = await getNations(worldId);
                break;
        }
        setCandidates(data);
    };

    const openForm = () => {
        setOpen(true);
        setError(null);
        setNote("");
        loadCandidates(entityType);
    };

    const onTypeChange = (type: EntityType) => {
        setEntityType(type);
        loadCandidates(type);
    };

    async function onSubmit() {
        if (!pick) return;
        setBusy(true);
        setError(null);
        try {
            await createReference({
                note,
                contentId: contentId ?? null,
                chapterId: chapterId ?? null,
                episodeId: episodeId ?? null,
                characterId: entityType === "Character" ? Number(pick) : null,
                locationId: entityType === "Location" ? Number(pick) : null,
                factionId: entityType === "Faction" ? Number(pick) : null,
                nationId: entityType === "Nation" ? Number(pick) : null,
            });
            setOpen(false);
            refetch();
        } catch (err) {
            console.error("Failed to add reference:", err);
            setError(apiErrorMessage(err, t("references.addFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onRemove(id: number) {
        setBusy(true);
        setError(null);
        try {
            await deleteReference(id);
            refetch();
        } catch (err) {
            console.error("Failed to remove reference:", err);
            setError(apiErrorMessage(err, t("references.removeFailed")));
        } finally {
            setBusy(false);
        }
    }

    return (
        <div>
            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("references.label")}</span>
                <span className={s.sectionLine} />
                {canEdit && !open && (
                    <button type="button" className={s.addInline} onClick={openForm}>
                        + {t("references.add")}
                    </button>
                )}
            </div>
            {loading ? (
                <p className={s.none}>{t("loaderror", { defaultValue: "…" })}</p>
            ) : references.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                <div className={s.chips}>
                    {references.map((r) => {
                        const target = entityTypeAndId(r);
                        if (!target) return null;
                        const name = names[`${target.type}:${target.id}`] ?? `#${target.id}`;
                        return (
                            <span key={r.id} className={s.chipRow}>
                                <Link
                                    to={`/storymap/${entityRoutes[target.type]}/${target.id}`}
                                    className={s.chipLink}
                                >
                                    <Tag tone="neutral">
                                        {t(`references.types.${target.type}`)}
                                        {" · "}
                                        {name}
                                    </Tag>
                                </Link>
                                {canEdit && (
                                    <button
                                        type="button"
                                        className={s.chipRemove}
                                        aria-label={t("references.remove", { name })}
                                        disabled={busy}
                                        onClick={() => onRemove(r.id)}
                                    >
                                        ×
                                    </button>
                                )}
                            </span>
                        );
                    })}
                </div>
            )}
            {open && (
                <div className={s.form}>
                    <OrnateField label={t("references.entityType")}>
                        <OrnateSelect
                            value={entityType}
                            onChange={(e) =>
                                onTypeChange(e.target.value as EntityType)
                            }
                        >
                            {entityTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`references.types.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("references.pick")}>
                        <OrnateSelect
                            value={pick}
                            onChange={(e) => setPick(e.target.value)}
                        >
                            <option value="">—</option>
                            {(candidates ?? []).map((c) => (
                                <option key={c.id} value={c.id}>
                                    {c.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("references.note")}>
                        <OrnateTextInput
                            value={note}
                            maxLength={1000}
                            onChange={(e) => setNote(e.target.value)}
                        />
                    </OrnateField>
                    {error && (
                        <p className={s.error} role="alert">
                            {error}
                        </p>
                    )}
                    <div className={s.actions}>
                        <Button
                            variant="ghost"
                            size="sm"
                            disabled={busy}
                            onClick={() => setOpen(false)}
                        >
                            {t("references.cancel")}
                        </Button>
                        <Button
                            size="sm"
                            disabled={busy || !pick}
                            onClick={onSubmit}
                        >
                            {t("references.confirm")}
                        </Button>
                    </div>
                </div>
            )}
        </div>
    );
}
