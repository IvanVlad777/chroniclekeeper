import { useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateField,
    OrnateTextArea,
    OrnateTextInput,
} from "../ornate";
import { createWorld } from "../../api/worlds";
import { WorldDto } from "../../interfaces/loreInterfaces";
import { apiErrorMessage } from "../../utils/apiError";
import s from "./shell.module.css";

interface CreateWorldDialogProps {
    onCreated: (world: WorldDto) => void;
    onClose: () => void;
}

export function CreateWorldDialog({ onCreated, onClose }: CreateWorldDialogProps) {
    const { t } = useTranslation();
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!name.trim()) {
            setError(t("worldDialog.nameRequired"));
            return;
        }
        setError(null);
        setBusy(true);
        try {
            const world = await createWorld({
                name: name.trim(),
                description: description.trim(),
            });
            onCreated(world);
        } catch (err) {
            console.error("Failed to create world:", err);
            setError(apiErrorMessage(err, t("worldDialog.failed")));
        } finally {
            setBusy(false);
        }
    }

    return (
        <div
            className={s.overlay}
            onClick={(e) => e.target === e.currentTarget && onClose()}
        >
            <form
                className={s.dialog}
                role="dialog"
                aria-modal="true"
                aria-label={t("worldDialog.title")}
                onSubmit={onSubmit}
            >
                <h2 className={s.dialogTitle}>{t("worldDialog.title")}</h2>
                <p className={s.dialogSub}>{t("worldDialog.sub")}</p>
                <div className={s.dialogFields}>
                    <OrnateField label={t("worldDialog.name")} required>
                        <OrnateTextInput
                            value={name}
                            display
                            autoFocus
                            maxLength={100}
                            onChange={(e) => setName(e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("worldDialog.description")}>
                        <OrnateTextArea
                            value={description}
                            rows={3}
                            maxLength={4000}
                            onChange={(e) => setDescription(e.target.value)}
                        />
                    </OrnateField>
                    {error && (
                        <p className={s.dialogError} role="alert">
                            {error}
                        </p>
                    )}
                </div>
                <div className={s.dialogActions}>
                    <Button variant="ghost" onClick={onClose} disabled={busy}>
                        {t("worldDialog.cancel")}
                    </Button>
                    <Button type="submit" disabled={busy}>
                        {busy ? t("worldDialog.creating") : t("worldDialog.create")}
                    </Button>
                </div>
            </form>
        </div>
    );
}
