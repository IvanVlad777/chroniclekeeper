import { useCallback, useEffect, useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../feedback";
import { NoteDto } from "../../../interfaces/loreInterfaces";
import {
    createNote,
    deleteNote,
    getNotes,
    updateNote,
} from "../../../api/notes";
import { useWorld } from "../../../hooks/useWorld";
import { useAuth } from "../../../hooks/useAuth";
import { apiErrorMessage } from "../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

/** Split-pane: lijevo popis bilješki, desno editor. Selekcija je lokalni state. */
export default function NotesPage() {
    const { t } = useTranslation("note");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [notes, setNotes] = useState<NoteDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // Odabrana bilješka: null = ništa, 0 = nova, >0 = id
    const [selectedId, setSelectedId] = useState<number | null>(null);
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [content, setContent] = useState("");
    const [formError, setFormError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setNotes([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getNotes(selectedWorld.id)
            .then((data) => {
                if (!cancelled) {
                    // najsvježije gore
                    setNotes(
                        [...data].sort((a, b) =>
                            (b.updatedAt ?? "").localeCompare(
                                a.updatedAt ?? ""
                            )
                        )
                    );
                }
            })
            .catch((err) => {
                console.error("Failed to load notes:", err);
                if (!cancelled) setError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, t, reloadKey]);

    const selectNote = (note: NoteDto) => {
        setSelectedId(note.id);
        setName(note.name);
        setDescription(note.description ?? "");
        setContent(note.content ?? "");
        setFormError(null);
    };

    const openNew = () => {
        setSelectedId(0);
        setName("");
        setDescription("");
        setContent("");
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
                const created = await createNote({
                    name: name.trim(),
                    description,
                    content,
                    worldId: selectedWorld.id,
                });
                setSelectedId(created.id);
            } else {
                await updateNote(selectedId, {
                    name: name.trim(),
                    description,
                    content,
                });
            }
            refetch();
        } catch (err) {
            console.error("Failed to save note:", err);
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
            await deleteNote(selectedId);
            setSelectedId(null);
            refetch();
        } catch (err) {
            console.error("Failed to delete note:", err);
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
                glyph="✎"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }

    return (
        <div className={s.page}>
            <div className={s.listPane}>
                <div className={s.listHead}>
                    <h1 className={s.listTitle}>{t("listTitle")}</h1>
                    <span className={s.count}>{notes.length}</span>
                    <span className={s.spacer} />
                    {canEdit && (
                        <button
                            type="button"
                            className={s.addBtn}
                            title={t("newNote")}
                            aria-label={t("newNote")}
                            onClick={openNew}
                        >
                            +
                        </button>
                    )}
                </div>
                {notes.length === 0 ? (
                    <p className={s.listEmpty}>{t("emptyText")}</p>
                ) : (
                    notes.map((note) => (
                        <button
                            key={note.id}
                            type="button"
                            className={`${s.noteRow} ${
                                note.id === selectedId ? s.noteRowActive : ""
                            }`}
                            onClick={() => selectNote(note)}
                        >
                            <span className={s.noteRowHead}>
                                <span className={s.noteTitle}>
                                    {note.name || t("untitled")}
                                </span>
                                <span className={s.noteDate}>
                                    {note.updatedAt
                                        ? new Date(
                                              note.updatedAt
                                          ).toLocaleDateString()
                                        : ""}
                                </span>
                            </span>
                            {(note.description || note.content) && (
                                <span className={s.noteSnippet}>
                                    {note.description || note.content}
                                </span>
                            )}
                        </button>
                    ))
                )}
            </div>

            <div className={s.editorPane}>
                {selectedId === null ? (
                    <p className={s.pickHint}>{t("pickHint")}</p>
                ) : (
                    <>
                        <div className={s.editorKicker}>
                            {selectedId === 0
                                ? t("newNote")
                                : t("listTitle")}
                        </div>
                        <form className={s.editorForm} onSubmit={onSave}>
                            <OrnateField label={t("fields.title")} required>
                                <OrnateTextInput
                                    value={name}
                                    display
                                    maxLength={100}
                                    disabled={!canEdit}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.description")}>
                                <OrnateTextInput
                                    value={description}
                                    maxLength={4000}
                                    disabled={!canEdit}
                                    onChange={(e) =>
                                        setDescription(e.target.value)
                                    }
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.content")}>
                                <OrnateTextArea
                                    value={content}
                                    rows={14}
                                    disabled={!canEdit}
                                    onChange={(e) =>
                                        setContent(e.target.value)
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
