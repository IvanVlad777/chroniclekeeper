import s from "./feedback.module.css";

export interface LoadingSkeletonProps {
    variant?: "table" | "block";
    rows?: number;
}

/** Shimmer placeholder dok se sadržaj učitava (Grimoire loading mockup). */
export function LoadingSkeleton({
    variant = "block",
    rows = 5,
}: LoadingSkeletonProps) {
    if (variant === "table") {
        return (
            <div className={s.skeleton} aria-busy="true">
                <div className={s.skeletonTitle} />
                <div className={s.skeletonTable}>
                    <div className={s.skeletonHead}>
                        {Array.from({ length: 4 }, (_, i) => (
                            <div key={i} className={s.skeletonHeadCell} />
                        ))}
                    </div>
                    {Array.from({ length: rows }, (_, i) => (
                        <div key={i} className={s.skeletonRow}>
                            <div className={s.skeletonCellLead} />
                            <div className={s.skeletonCell} />
                            <div className={s.skeletonPill} />
                            <div className={s.skeletonCell} />
                        </div>
                    ))}
                </div>
            </div>
        );
    }

    return (
        <div className={s.skeleton} aria-busy="true">
            <div className={s.skeletonTitle} />
            {Array.from({ length: rows }, (_, i) => (
                <div
                    key={i}
                    className={s.skeletonBlockLine}
                    style={{ width: `${90 - i * 12}%` }}
                />
            ))}
        </div>
    );
}
