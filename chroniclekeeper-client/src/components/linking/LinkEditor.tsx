import { useEffect, useRef, useState, type FormEvent } from "react";
import { Link } from "react-router-dom";
import { Button, OrnateField, OrnateSelect, Tag } from "../ornate";
import { ReferenceDto } from "../../interfaces/loreInterfaces";
import { apiErrorMessage } from "../../utils/apiError";
import s from "./linkEditor.module.css";

export interface LinkEditorProps {
    /** Trenutno povezani zapisi (iz details DTO-a). */
    items: ReferenceDto[];
    /** Kandidati za dodavanje — null dok se ne učitaju (lijeno, tek kad se forma otvori). */
    candidates: ReferenceDto[] | null;
    onLoadCandidates: () => void;
    onAdd: (id: number) => Promise<void>;
    onRemove: (id: number) => Promise<void>;
    /** Nakon uspješnog add/remove — roditelj refetcha details. */
    onChanged: () => void;
    canEdit: boolean;
    linkTo: (id: number) => string;
    addLabel: string;
    /** Aria-label za × gumb, npr. (name) => `Ukloni ${name}`. */
    removeLabel: (name: string) => string;
    noneLabel: string;
    pickLabel: string;
    cancelLabel: string;
    confirmLabel: string;
    addFailedLabel: string;
    removeFailedLabel: string;
}

/**
 * Generički editor za many-to-many veze na detaljima entiteta (Culture↔Nation
 * i sl.): chipovi povezanih zapisa (svaki link na svoj detalj) + uklanjanje +
 * "+ dodaj" forma s odabirom iz kandidata svijeta.
 */
export function LinkEditor({
    items,
    candidates,
    onLoadCandidates,
    onAdd,
    onRemove,
    onChanged,
    canEdit,
    linkTo,
    addLabel,
    removeLabel,
    noneLabel,
    pickLabel,
    cancelLabel,
    confirmLabel,
    addFailedLabel,
    removeFailedLabel,
}: LinkEditorProps) {
    const [open, setOpen] = useState(false);
    const [pick, setPick] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const requestedRef = useRef(false);

    useEffect(() => {
        if (!open || candidates !== null || requestedRef.current) return;
        requestedRef.current = true;
        onLoadCandidates();
    }, [open, candidates, onLoadCandidates]);

    const available = (candidates ?? []).filter(
        (c) => !items.some((i) => i.id === c.id)
    );

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!pick) return;
        setError(null);
        setBusy(true);
        try {
            await onAdd(Number(pick));
            setOpen(false);
            setPick("");
            onChanged();
        } catch (err) {
            console.error("Failed to add link:", err);
            setError(apiErrorMessage(err, addFailedLabel));
        } finally {
            setBusy(false);
        }
    }

    async function handleRemove(id: number) {
        setError(null);
        setBusy(true);
        try {
            await onRemove(id);
            onChanged();
        } catch (err) {
            console.error("Failed to remove link:", err);
            setError(apiErrorMessage(err, removeFailedLabel));
            setBusy(false);
        }
    }

    return (
        <div className={s.wrap}>
            <div className={s.chips}>
                {items.length === 0 && !canEdit && (
                    <p className={s.none}>{noneLabel}</p>
                )}
                {items.map((item) => (
                    <span key={item.id} className={s.chipRow}>
                        <Link to={linkTo(item.id)} className={s.chipLink}>
                            <Tag tone="neutral">{item.name}</Tag>
                        </Link>
                        {canEdit && (
                            <button
                                type="button"
                                className={s.chipRemove}
                                aria-label={removeLabel(item.name)}
                                disabled={busy}
                                onClick={() => handleRemove(item.id)}
                            >
                                ×
                            </button>
                        )}
                    </span>
                ))}
                {canEdit && !open && (
                    <button
                        type="button"
                        className={s.addChip}
                        onClick={() => setOpen(true)}
                    >
                        + {addLabel}
                    </button>
                )}
            </div>
            {open && (
                <form className={s.form} onSubmit={onSubmit}>
                    <OrnateField label={pickLabel}>
                        <OrnateSelect
                            value={pick}
                            onChange={(e) => setPick(e.target.value)}
                        >
                            <option value="">—</option>
                            {available.map((c) => (
                                <option key={c.id} value={c.id}>
                                    {c.name}
                                </option>
                            ))}
                        </OrnateSelect>
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
                            {cancelLabel}
                        </Button>
                        <Button type="submit" size="sm" disabled={busy || !pick}>
                            {confirmLabel}
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
