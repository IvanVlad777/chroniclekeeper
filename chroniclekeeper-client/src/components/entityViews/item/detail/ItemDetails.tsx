import { useCallback, useEffect, useState, type FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    DisplayGrid,
    OrnateDisplayBox,
    OrnateField,
    OrnateSelect,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    CharacterDto,
    ItemDetailsDto,
    OwnershipHistoryDto,
    OwnershipTransferReason,
    ownershipTransferReasons,
} from "../../../../interfaces/loreInterfaces";
import {
    createOwnershipHistory,
    deleteOwnershipHistory,
    getItemById,
    updateOwnershipHistory,
} from "../../../../api/items";
import { getCharacters } from "../../../../api/characters";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⚔";

interface OwnershipHistoryFormState {
    name: string;
    description: string;
    previousOwnerId: string;
    newOwnerId: string;
    dateAcquired: string;
    transferReason: OwnershipTransferReason;
}
const emptyHistoryForm: OwnershipHistoryFormState = {
    name: "",
    description: "",
    previousOwnerId: "",
    newOwnerId: "",
    dateAcquired: "",
    transferReason: "Found",
};

export default function ItemDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("item");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<ItemDetailsDto | null>(null);
    const [charactersForForm, setCharactersForForm] = useState<
        CharacterDto[]
    >([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [busy, setBusy] = useState(false);

    // vlasnički zapis: null = zatvoreno, 0 = novi, >0 = edit tog id-a
    const [historyFormFor, setHistoryFormFor] = useState<number | null>(null);
    const [historyForm, setHistoryForm] =
        useState<OwnershipHistoryFormState>(emptyHistoryForm);
    const [historyError, setHistoryError] = useState<string | null>(null);

    useEffect(() => {
        const itemId = Number(id);
        if (!itemId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getItemById(itemId)
            .then((data) => {
                if (cancelled) return;
                setItem(data);
                return getCharacters(data.worldId);
            })
            .then((chars) => {
                if (!cancelled && chars) setCharactersForForm(chars);
            })
            .catch((err) => {
                console.error("Failed to load item:", err);
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

    const openNewHistory = () => {
        setHistoryForm(emptyHistoryForm);
        setHistoryError(null);
        setHistoryFormFor(0);
    };
    const openEditHistory = (h: OwnershipHistoryDto) => {
        setHistoryForm({
            name: h.name ?? "",
            description: h.description ?? "",
            previousOwnerId: h.previousOwnerId ? String(h.previousOwnerId) : "",
            newOwnerId: h.newOwnerId ? String(h.newOwnerId) : "",
            dateAcquired: h.dateAcquired ?? "",
            transferReason: (h.transferReason as OwnershipTransferReason) ?? "Found",
        });
        setHistoryError(null);
        setHistoryFormFor(h.id);
    };
    async function onSaveHistory(e: FormEvent) {
        e.preventDefault();
        if (!item || historyFormFor === null) return;
        if (!historyForm.name.trim()) {
            setHistoryError(t("history.requiredMissing"));
            return;
        }
        setHistoryError(null);
        setBusy(true);
        try {
            const payload = {
                name: historyForm.name.trim(),
                description: historyForm.description,
                previousOwnerId: historyForm.previousOwnerId
                    ? Number(historyForm.previousOwnerId)
                    : null,
                newOwnerId: historyForm.newOwnerId
                    ? Number(historyForm.newOwnerId)
                    : null,
                dateAcquired: historyForm.dateAcquired,
                transferReason: historyForm.transferReason,
            };
            if (historyFormFor === 0) {
                await createOwnershipHistory({ ...payload, itemId: item.id });
            } else {
                await updateOwnershipHistory(historyFormFor, payload);
            }
            setHistoryFormFor(null);
            refetch();
        } catch (err) {
            console.error("Failed to save ownership history:", err);
            setHistoryError(apiErrorMessage(err, t("history.saveFailed")));
        } finally {
            setBusy(false);
        }
    }
    async function onDeleteHistory(historyId: number, name: string) {
        if (!window.confirm(t("history.deleteConfirm", { name }))) return;
        setHistoryError(null);
        setBusy(true);
        try {
            await deleteOwnershipHistory(historyId);
            refetch();
        } catch (err) {
            console.error("Failed to delete ownership history:", err);
            setHistoryError(apiErrorMessage(err, t("history.deleteFailed")));
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
                    <Link to="/storymap/items" className={s.backLink}>
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !item) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/items">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(`/storymap/items/${item.id}/edit`)
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={2}>
                    <OrnateDisplayBox
                        label={t("columns.category")}
                        value={item.category ? t(`categories.${item.category}`) : dash}
                    />
                    <OrnateDisplayBox
                        label={t("columns.rarity")}
                        value={item.rarity ? t(`rarities.${item.rarity}`) : dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.material")}
                        value={item.material || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isUnique")}
                        value={item.isUnique ? t("yes") : t("no")}
                    />
                    <OrnateDisplayBox
                        label={t("fields.currentOwner")}
                        value={
                            item.currentOwner ? (
                                <Link to={`/storymap/characters/${item.currentOwner.id}`}>
                                    {item.currentOwner.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.storedAt")}
                        value={
                            item.storedAt ? (
                                <Link to={`/storymap/locations/${item.storedAt.id}`}>
                                    {item.storedAt.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.faction")}
                        value={
                            item.faction ? (
                                <Link to={`/storymap/factions/${item.faction.id}`}>
                                    {item.faction.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.specialProperties")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.specialProperties ? (
                <p className={`${s.prose} ${s.dropCap}`}>{item.specialProperties}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={s.prose}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* ----- Ownership history ----- */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("history.label")}</span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
                {canEdit && historyFormFor === null && (
                    <button type="button" className={s.addInline} onClick={openNewHistory}>
                        + {t("history.add")}
                    </button>
                )}
            </div>
            {item.ownershipHistory.length === 0 && historyFormFor === null ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                item.ownershipHistory.map((h) => (
                    <div key={h.id} className={s.childRow}>
                        <span className={s.childName}>{h.name}</span>
                        <div className={s.childBody}>
                            {h.description && <p className={s.childDesc}>{h.description}</p>}
                            <p className={s.childMeta}>
                                {[
                                    t(`history.reasons.${h.transferReason}`),
                                    h.dateAcquired,
                                ]
                                    .filter(Boolean)
                                    .join(" · ")}
                            </p>
                        </div>
                        {canEdit && (
                            <span className={s.childActions}>
                                <button type="button" className={s.childActionBtn} disabled={busy} onClick={() => openEditHistory(h)}>
                                    {t("form.edit")}
                                </button>
                                <button type="button" className={`${s.childActionBtn} ${s.childActionDanger}`} disabled={busy} onClick={() => onDeleteHistory(h.id, h.name)}>
                                    {t("history.delete")}
                                </button>
                            </span>
                        )}
                    </div>
                ))
            )}
            {historyError && historyFormFor === null && (
                <p className={s.miniError} role="alert">{historyError}</p>
            )}
            {historyFormFor !== null && (
                <form className={s.childForm} onSubmit={onSaveHistory}>
                    <h3 className={s.childFormTitle}>
                        {historyFormFor === 0 ? t("history.addTitle") : t("history.editTitle")}
                    </h3>
                    <OrnateField label={t("history.name")} required>
                        <OrnateTextInput value={historyForm.name} display maxLength={100} onChange={(e) => setHistoryForm((f) => ({ ...f, name: e.target.value }))} />
                    </OrnateField>
                    <div className={s.row2}>
                        <OrnateField label={t("history.previousOwner")}>
                            <OrnateSelect value={historyForm.previousOwnerId} onChange={(e) => setHistoryForm((f) => ({ ...f, previousOwnerId: e.target.value }))}>
                                <option value="">{t("none")}</option>
                                {charactersForForm.map((c) => (
                                    <option key={c.id} value={c.id}>{c.name}</option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                        <OrnateField label={t("history.newOwner")}>
                            <OrnateSelect value={historyForm.newOwnerId} onChange={(e) => setHistoryForm((f) => ({ ...f, newOwnerId: e.target.value }))}>
                                <option value="">{t("none")}</option>
                                {charactersForForm.map((c) => (
                                    <option key={c.id} value={c.id}>{c.name}</option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    <div className={s.row2}>
                        <OrnateField label={t("history.dateAcquired")}>
                            <OrnateTextInput value={historyForm.dateAcquired} maxLength={100} onChange={(e) => setHistoryForm((f) => ({ ...f, dateAcquired: e.target.value }))} />
                        </OrnateField>
                        <OrnateField label={t("history.transferReason")} required>
                            <OrnateSelect value={historyForm.transferReason} onChange={(e) => setHistoryForm((f) => ({ ...f, transferReason: e.target.value as OwnershipTransferReason }))}>
                                {ownershipTransferReasons.map((r) => (
                                    <option key={r} value={r}>{t(`history.reasons.${r}`)}</option>
                                ))}
                            </OrnateSelect>
                        </OrnateField>
                    </div>
                    {historyError && <p className={s.miniError} role="alert">{historyError}</p>}
                    <div className={s.miniActions}>
                        <Button variant="ghost" size="sm" disabled={busy} onClick={() => setHistoryFormFor(null)}>{t("history.cancel")}</Button>
                        <Button type="submit" size="sm" disabled={busy}>{t("history.save")}</Button>
                    </div>
                </form>
            )}
        </div>
    );
}
