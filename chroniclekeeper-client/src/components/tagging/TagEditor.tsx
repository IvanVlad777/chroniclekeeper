import { useEffect, useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateSelect,
    OrnateTextInput,
    Tag,
} from "../ornate";
import { attachTag, createTag, detachTag, getTags } from "../../api/tags";
import {
    ReferenceDto,
    TagDto,
    TagTargetType,
} from "../../interfaces/loreInterfaces";
import { apiErrorMessage } from "../../utils/apiError";
import s from "./tagEditor.module.css";

export interface TagEditorProps {
    worldId: number;
    targetType: TagTargetType;
    targetId: number;
    /** Trenutno zakačeni tagovi (iz details DTO-a). */
    tags: ReferenceDto[];
    canEdit: boolean;
    /** Poziva se nakon uspješnog attach/detach — roditelj refetcha details. */
    onChanged: () => void;
    /** Prikaži "Tags" naslov iznad chipova. */
    showLabel?: boolean;
}

/**
 * Chipovi tagova s uklanjanjem i "+ tag" formom (postojeći tag ili novi).
 * Listu tagova svijeta učitava lijeno — tek kad se forma otvori.
 */
export function TagEditor({
    worldId,
    targetType,
    targetId,
    tags,
    canEdit,
    onChanged,
    showLabel = false,
}: TagEditorProps) {
    const { t } = useTranslation();
    const [open, setOpen] = useState(false);
    const [worldTags, setWorldTags] = useState<TagDto[] | null>(null);
    const [pick, setPick] = useState("");
    const [newName, setNewName] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    useEffect(() => {
        if (!open || worldTags !== null) return;
        let cancelled = false;
        getTags(worldId)
            .then((data) => {
                if (!cancelled) setWorldTags(data);
            })
            .catch((err) => console.error("Failed to load tags:", err));
        return () => {
            cancelled = true;
        };
    }, [open, worldTags, worldId]);

    const available = (worldTags ?? []).filter(
        (wt) => !tags.some((ct) => ct.id === wt.id)
    );

    async function onAttach(e: FormEvent) {
        e.preventDefault();
        setError(null);
        setBusy(true);
        try {
            let tagId: number;
            if (pick) {
                tagId = Number(pick);
            } else if (newName.trim()) {
                const created = await createTag({
                    name: newName.trim(),
                    description: "",
                    worldId,
                });
                tagId = created.id;
            } else {
                setBusy(false);
                return;
            }
            await attachTag(tagId, { targetType, targetId });
            setOpen(false);
            setPick("");
            setNewName("");
            setWorldTags(null); // možda je stvoren novi tag — osvježi idući put
            onChanged();
        } catch (err) {
            console.error("Failed to attach tag:", err);
            setError(apiErrorMessage(err, t("tagEditor.addFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDetach(tagId: number) {
        setError(null);
        setBusy(true);
        try {
            await detachTag(tagId, targetType, targetId);
            onChanged();
        } catch (err) {
            console.error("Failed to detach tag:", err);
            setError(apiErrorMessage(err, t("tagEditor.removeFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (!canEdit && tags.length === 0) {
        return showLabel ? (
            <div className={s.wrap}>
                <div className={s.label}>{t("tagEditor.label")}</div>
                <p className={s.none}>—</p>
            </div>
        ) : null;
    }

    return (
        <div className={s.wrap}>
            {showLabel && (
                <div className={s.label}>{t("tagEditor.label")}</div>
            )}
            <div className={s.chips}>
                {tags.map((tag) => (
                    <Tag key={tag.id}>
                        {tag.name}
                        {canEdit && (
                            <button
                                type="button"
                                className={s.chipRemove}
                                aria-label={t("tagEditor.remove", {
                                    name: tag.name,
                                })}
                                disabled={busy}
                                onClick={() => onDetach(tag.id)}
                            >
                                ×
                            </button>
                        )}
                    </Tag>
                ))}
                {canEdit && !open && (
                    <button
                        type="button"
                        className={s.addChip}
                        onClick={() => setOpen(true)}
                    >
                        + {t("tagEditor.add")}
                    </button>
                )}
            </div>
            {open && (
                <form className={s.form} onSubmit={onAttach}>
                    <OrnateField label={t("tagEditor.pick")}>
                        <OrnateSelect
                            value={pick}
                            onChange={(e) => {
                                setPick(e.target.value);
                                if (e.target.value) setNewName("");
                            }}
                        >
                            <option value="">—</option>
                            {available.map((tag) => (
                                <option key={tag.id} value={tag.id}>
                                    {tag.name}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("tagEditor.orNew")}>
                        <OrnateTextInput
                            value={newName}
                            maxLength={50}
                            onChange={(e) => {
                                setNewName(e.target.value);
                                if (e.target.value) setPick("");
                            }}
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
                            onClick={() => {
                                setOpen(false);
                                setError(null);
                            }}
                        >
                            {t("tagEditor.cancel")}
                        </Button>
                        <Button
                            type="submit"
                            size="sm"
                            disabled={busy || (!pick && !newName.trim())}
                        >
                            {t("tagEditor.attach")}
                        </Button>
                    </div>
                </form>
            )}
            {!open && error && (
                <p className={s.error} role="alert">
                    {error}
                </p>
            )}
        </div>
    );
}
