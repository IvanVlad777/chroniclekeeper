import type { ReactNode } from "react";
import s from "./feedback.module.css";

export interface EmptyStateProps {
    glyph?: string;
    title: ReactNode;
    text?: ReactNode;
    /** npr. "+ Create" gumb */
    action?: ReactNode;
}

export function EmptyState({ glyph = "✦", title, text, action }: EmptyStateProps) {
    return (
        <div className={s.empty}>
            <div className={s.emptyGlyph}>{glyph}</div>
            <h2 className={s.emptyTitle}>{title}</h2>
            {text && <p className={s.emptyText}>{text}</p>}
            {action}
        </div>
    );
}
