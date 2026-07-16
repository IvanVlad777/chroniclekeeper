import { useTranslation } from "react-i18next";
import { navDomains, type NavDomain } from "./navConfig";
import s from "./shell.module.css";

interface RailProps {
    activeDomainKey: string;
    onSelect: (domain: NavDomain) => void;
}

/** 64px icon rail of the 12 domains. Selecting a domain switches the panel. */
export function Rail({ activeDomainKey, onSelect }: RailProps) {
    const { t } = useTranslation();

    return (
        <div className={s.rail}>
            {navDomains.map((domain) => {
                const active = domain.key === activeDomainKey;
                const label = t(`navGroups.${domain.key}`);
                return (
                    <button
                        key={domain.key}
                        type="button"
                        className={`${s.railBtn} ${active ? s.railBtnActive : ""}`}
                        title={label}
                        aria-label={label}
                        aria-current={active ? "true" : undefined}
                        onClick={() => onSelect(domain)}
                    >
                        {domain.glyph}
                    </button>
                );
            })}
        </div>
    );
}
