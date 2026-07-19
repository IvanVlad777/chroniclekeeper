import { Link } from "react-router-dom";
import { Tag } from "../ornate";
import { ReferenceDto } from "../../interfaces/loreInterfaces";
import s from "./linkEditor.module.css";

interface ReadRefListProps {
    items: ReferenceDto[];
    /** Omit for targets without a dedicated detail page — the chip renders as plain text. */
    linkTo?: (id: number) => string;
    noneLabel: string;
}

/**
 * Read-only counterpart to LinkEditor: a chip list of related entries (each optionally a link to
 * its own detail page) with a "none" fallback. For reverse relationships whose write side lives on
 * another entity (e.g. a Hobby's practitioners, a Clothing's wearers).
 */
export function ReadRefList({ items, linkTo, noneLabel }: ReadRefListProps) {
    return (
        <div className={s.chips}>
            {items.length === 0 ? (
                <p className={s.none}>{noneLabel}</p>
            ) : (
                items.map((item) => (
                    <span key={item.id} className={s.chipRow}>
                        {linkTo ? (
                            <Link to={linkTo(item.id)} className={s.chipLink}>
                                <Tag tone="neutral">{item.name}</Tag>
                            </Link>
                        ) : (
                            <Tag tone="neutral">{item.name}</Tag>
                        )}
                    </span>
                ))
            )}
        </div>
    );
}
