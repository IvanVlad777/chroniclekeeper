import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    createContent,
    deleteContent,
    getContentById,
    getContents,
    updateContent,
} from "../../../../api/contents";
import {
    ContentDto,
    ContentType,
    ContentUpdateDto,
    contentTypes,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    type: ContentType;
    source: string;
    publishDate: string;
    author: string;
    releaseDate: string;
    issueNumber: string;
    director: string;
    durationMinutes: string;
    prequelId: string;
    creator: string;
    seasons: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    type: "Book",
    source: "",
    publishDate: "",
    author: "",
    releaseDate: "",
    issueNumber: "",
    director: "",
    durationMinutes: "",
    prequelId: "",
    creator: "",
    seasons: "",
};

const toNum = (v: string): number | null => (v.trim() ? Number(v) : null);
const toDate = (v: string): string | null => (v ? v : null);

function toDto(f: FormState): ContentUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        source: f.type === "Article" ? f.source : null,
        publishDate: f.type === "Article" ? toDate(f.publishDate) : null,
        author: f.type === "Book" || f.type === "Comic" ? f.author : null,
        releaseDate:
            f.type === "Book" || f.type === "Movie"
                ? toDate(f.releaseDate)
                : null,
        issueNumber: f.type === "Comic" ? toNum(f.issueNumber) : null,
        director: f.type === "Movie" ? f.director : null,
        durationMinutes:
            f.type === "Movie" ? toNum(f.durationMinutes) : null,
        prequelId: f.type === "Movie" ? toNum(f.prequelId) : null,
        creator: f.type === "Series" ? f.creator : null,
        seasons: f.type === "Series" ? toNum(f.seasons) : null,
    };
}

/** Zajednička forma za /contents/new i /contents/:id/edit — type selektira TPH podtip. */
export default function ContentForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("content");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canDelete =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [movieCandidates, setMovieCandidates] = useState<ContentDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        getContents({ worldId: selectedWorld.id, type: "Movie" })
            .then(async (movies) => {
                if (cancelled) return;
                setMovieCandidates(movies);
                if (isEdit) {
                    const c = await getContentById(editId);
                    if (cancelled) return;
                    setForm({
                        name: c.name ?? "",
                        description: c.description ?? "",
                        type: (c.type as ContentType) ?? "Book",
                        source: c.source ?? "",
                        publishDate: c.publishDate ? c.publishDate.slice(0, 10) : "",
                        author: c.author ?? "",
                        releaseDate: c.releaseDate ? c.releaseDate.slice(0, 10) : "",
                        issueNumber: c.issueNumber != null ? String(c.issueNumber) : "",
                        director: c.director ?? "",
                        durationMinutes:
                            c.durationMinutes != null ? String(c.durationMinutes) : "",
                        prequelId: c.prequelId != null ? String(c.prequelId) : "",
                        creator: c.creator ?? "",
                        seasons: c.seasons != null ? String(c.seasons) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load content form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateContent(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createContent({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                    type: form.type,
                });
                targetId = created.id;
            }
            navigate(`/storymap/contents/${targetId}`);
        } catch (err) {
            console.error("Failed to save content:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) {
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            await deleteContent(editId);
            navigate("/storymap/contents");
        } catch (err) {
            console.error("Failed to delete content:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="📖"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }
    if (loadError) {
        return (
            <ErrorState
                onRetry={() => setReloadKey((k) => k + 1)}
                detail={loadError}
            />
        );
    }

    return (
        <form className={s.page} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>
                {isEdit ? t("form.editTitle") : t("form.newTitle")}
            </h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("columns.type")} required>
                        <OrnateSelect
                            value={form.type}
                            disabled={isEdit}
                            onChange={(e) =>
                                set("type", e.target.value as ContentType)
                            }
                        >
                            {contentTypes.map((type) => (
                                <option key={type} value={type}>
                                    {t(`types.${type}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("columns.name")} required>
                        <OrnateTextInput
                            value={form.name}
                            display
                            maxLength={100}
                            onChange={(e) => set("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={8}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                </div>

                <div className={s.col}>
                    {form.type === "Article" && (
                        <>
                            <OrnateField label={t("fields.source")}>
                                <OrnateTextInput
                                    value={form.source}
                                    maxLength={200}
                                    onChange={(e) => set("source", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.publishDate")}>
                                <OrnateTextInput
                                    type="date"
                                    value={form.publishDate}
                                    onChange={(e) => set("publishDate", e.target.value)}
                                />
                            </OrnateField>
                        </>
                    )}
                    {(form.type === "Book" || form.type === "Comic") && (
                        <OrnateField label={t("fields.author")}>
                            <OrnateTextInput
                                value={form.author}
                                maxLength={100}
                                onChange={(e) => set("author", e.target.value)}
                            />
                        </OrnateField>
                    )}
                    {form.type === "Book" && (
                        <OrnateField label={t("fields.releaseDate")}>
                            <OrnateTextInput
                                type="date"
                                value={form.releaseDate}
                                onChange={(e) => set("releaseDate", e.target.value)}
                            />
                        </OrnateField>
                    )}
                    {form.type === "Comic" && (
                        <OrnateField label={t("fields.issueNumber")}>
                            <OrnateTextInput
                                type="number"
                                value={form.issueNumber}
                                onChange={(e) => set("issueNumber", e.target.value)}
                            />
                        </OrnateField>
                    )}
                    {form.type === "Movie" && (
                        <>
                            <OrnateField label={t("fields.director")}>
                                <OrnateTextInput
                                    value={form.director}
                                    maxLength={100}
                                    onChange={(e) => set("director", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.releaseDate")}>
                                <OrnateTextInput
                                    type="date"
                                    value={form.releaseDate}
                                    onChange={(e) => set("releaseDate", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.durationMinutes")}>
                                <OrnateTextInput
                                    type="number"
                                    min={0}
                                    value={form.durationMinutes}
                                    onChange={(e) =>
                                        set("durationMinutes", e.target.value)
                                    }
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.prequel")}>
                                <OrnateSelect
                                    value={form.prequelId}
                                    onChange={(e) => set("prequelId", e.target.value)}
                                >
                                    <option value="">{t("none")}</option>
                                    {movieCandidates
                                        .filter((m) => m.id !== editId)
                                        .map((m) => (
                                            <option key={m.id} value={m.id}>
                                                {m.name}
                                            </option>
                                        ))}
                                </OrnateSelect>
                            </OrnateField>
                        </>
                    )}
                    {form.type === "Series" && (
                        <>
                            <OrnateField label={t("fields.creator")}>
                                <OrnateTextInput
                                    value={form.creator}
                                    maxLength={100}
                                    onChange={(e) => set("creator", e.target.value)}
                                />
                            </OrnateField>
                            <OrnateField label={t("fields.seasons")}>
                                <OrnateTextInput
                                    type="number"
                                    min={0}
                                    value={form.seasons}
                                    onChange={(e) => set("seasons", e.target.value)}
                                />
                            </OrnateField>
                        </>
                    )}
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button
                        variant="danger"
                        disabled={busy}
                        onClick={onDelete}
                    >
                        {t("form.delete")}
                    </Button>
                )}
                <span className={s.footerSpacer} />
                <Button
                    variant="ghost"
                    disabled={busy}
                    onClick={() =>
                        navigate(
                            isEdit
                                ? `/storymap/contents/${editId}`
                                : "/storymap/contents"
                        )
                    }
                >
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>
                    {busy ? t("form.saving") : t("form.save")}
                </Button>
            </div>
        </form>
    );
}
