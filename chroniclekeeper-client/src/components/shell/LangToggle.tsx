import { useTranslation } from "react-i18next";
import s from "./shell.module.css";

const languages = ["en", "hr"] as const;

export function LangToggle() {
    const { i18n, t } = useTranslation();
    const current = (i18n.resolvedLanguage ?? i18n.language ?? "en").slice(
        0,
        2
    );

    return (
        <div className={s.lang} role="group" aria-label={t("lang.label")}>
            {languages.map((lng) => (
                <button
                    key={lng}
                    type="button"
                    className={`${s.langBtn} ${
                        current === lng ? s.langActive : ""
                    }`}
                    aria-pressed={current === lng}
                    onClick={() => void i18n.changeLanguage(lng)}
                >
                    {lng.toUpperCase()}
                </button>
            ))}
        </div>
    );
}
