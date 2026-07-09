import { useCallback, useEffect, useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
    Tag,
} from "../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../feedback";
import { TagDto } from "../../../interfaces/loreInterfaces";
import { createTag, deleteTag, getTags, updateTag } from "../../../api/tags";
import { useWorld } from "../../../hooks/useWorld";
import { useAuth } from "../../../hooks/useAuth";
import { apiErrorMessage } from "../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

/**
 * Tags index: grid chipova + panel s detaljima/uređivanjem odabrane oznake.
 * "Zapisi s ovom oznakom" čeka backend endpoint (GET /tags/{id}/attachments).
 */
export default function TagsPage() {
    const { t } = useTranslation("tag");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [tags, setTags] = useState<TagDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Odabrana oznaka: null = ništa, 0 = nova, >0 = id
    const [selectedId, setSelectedId] = useState<number | null>(null);
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [formError, setFormError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setTags([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getTags(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setTags(data);
            })
            .catch((err) => {
                console.error("Failed to load tags:", err);
                if (!cancelled) setError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, t, reloadKey]);

    const selectTag = (tag: TagDto) => {
        setSelectedId(tag.id);
        setName(tag.name);
        setDescription(tag.description ?? "");
        setFormError(null);
    };

    const openNew = () => {
        setSelectedId(0);
        setName("");
        setDescription("");
        setFormError(null);
    };

    async function onSave(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld || selectedId === null) return;
        if (!name.trim()) {
            setFormError(t("form.requiredMissing"));
            return;
        }
        setFormError(null);
        setBusy(true);
        try {
            if (selectedId === 0) {
                const created = await createTag({
                    name: name.trim(),
                    description,
                    worldId: selectedWorld.id,
                });
                setSelectedId(created.id);
            } else {
                await updateTag(selectedId, {
                    name: name.trim(),
                    description,
                });
            }
            refetch();
        } catch (err) {
            console.error("Failed to save tag:", err);
            setFormError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!selectedId) return;
        if (!window.confirm(t("form.deleteConfirm", { name }))) return;
        setFormError(null);
        setBusy(true);
        try {
            await deleteTag(selectedId);
            setSelectedId(null);
            refetch();
        } catch (err) {
            console.error("Failed to delete tag:", err);
            setFormError(apiErrorMessage(err, t("form.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    if (worldLoading || loading) return <LoadingSkeleton variant="block" />;
    if (error) return <ErrorState onRetry={refetch} detail={error} />;
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="❧"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    return (
        <div className={s.page}>
            <div>
                <div className={s.headerRow}>
                    <h1 className={s.title}>{t("listTitle")}</h1>
                    <span className={s.count}>{tags.length}</span>
                    <span className={s.spacer} />
                    {canEdit && (
                        <Button onClick={openNew}>+ {t("newTag")}</Button>
                    )}
                </div>
                {tags.length === 0 && selectedId !== 0 ? (
                    <EmptyState
                        glyph="❧"
                        title={t("emptyTitle")}
                        text={t("emptyText")}
                        action={
                            canEdit ? (
                                <Button onClick={openNew}>
                                    + {t("newTag")}
                                </Button>
                            ) : undefined
                        }
                    />
                ) : (
                    <>
                        <div className={s.chips}>
                            {tags.map((tag) => (
                                <Tag
                                    key={tag.id}
                                    interactive
                                    selected={tag.id === selectedId}
                                    onClick={() => selectTag(tag)}
                                >
                                    {tag.name}
                                </Tag>
                            ))}
                        </div>
                        <p className={s.attachNote}>{t("attachNote")}</p>
                    </>
                )}
            </div>

            <div className={s.panel}>
                {selectedId === null ? (
                    <p className={s.panelHint}>{t("detailHint")}</p>
                ) : (
                    <>
                        <div className={s.panelHead}>
                            <span className={s.panelDot} />
                            <span className={s.panelTitle}>
                                {selectedId === 0 ? t("newTag") : name}
                            </span>
                        </div>
                        <form className={s.form} onSubmit={onSave}>
                            <OrnateField label={t("fields.name")} required>
                                <OrnateTextInput
                                    value={name}
                                    maxLength={50}
                                    disabled={!canEdit}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.description")}>
                                <OrnateTextArea
                                    value={description}
                                    rows={4}
                                    maxLength={4000}
                                    disabled={!canEdit}
                                    onChange={(e) =>
                                        setDescription(e.target.value)
                                    }
                                />
                            </OrnateField>
                            {formError && (
                                <p className={s.formError} role="alert">
                                    {formError}
                                </p>
                            )}
                            {canEdit && (
                                <div className={s.actions}>
                                    {selectedId !== 0 && (
                                        <Button
                                            variant="danger"
                                            size="sm"
                                            disabled={busy}
                                            onClick={onDelete}
                                        >
                                            {t("form.delete")}
                                        </Button>
                                    )}
                                    <span className={s.actionsSpacer} />
                                    <Button
                                        variant="ghost"
                                        size="sm"
                                        disabled={busy}
                                        onClick={() => setSelectedId(null)}
                                    >
                                        {t("form.cancel")}
                                    </Button>
                                    <Button
                                        type="submit"
                                        size="sm"
                                        disabled={busy}
                                    >
                                        {busy
                                            ? t("form.saving")
                                            : selectedId === 0
                                            ? t("form.create")
                                            : t("form.save")}
                                    </Button>
                                </div>
                            )}
                        </form>
                    </>
                )}
            </div>
        </div>
    );
}
