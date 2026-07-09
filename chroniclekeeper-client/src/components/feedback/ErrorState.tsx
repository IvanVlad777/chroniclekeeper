import type { ReactNode } from "react";
import { useTranslation } from "react-i18next";
import { Button } from "../ornate";
import s from "./feedback.module.css";

export interface ErrorStateProps {
    title?: ReactNode;
    text?: ReactNode;
    /** Tehnički detalj (status, poruka) — prikazuje se sitno, monospace. */
    detail?: ReactNode;
    onRetry?: () => void;
}

export function ErrorState({ title, text, detail, onRetry }: ErrorStateProps) {
    const { t } = useTranslation();
    return (
        <div className={s.error} role="alert">
            <div className={s.errorGlyph}>⚠</div>
            <h2 className={s.errorTitle}>{title ?? t("states.errorTitle")}</h2>
            <p className={s.errorText}>{text ?? t("states.errorText")}</p>
            {onRetry && (
                <Button onClick={onRetry}>{t("states.retry")}</Button>
            )}
            {detail && <span className={s.errorDetail}>{detail}</span>}
        </div>
    );
}
