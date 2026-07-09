import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";

import enCommon from "./locales/en/common.json";
import hrCommon from "./locales/hr/common.json";
import enAuth from "./locales/en/auth.json";
import hrAuth from "./locales/hr/auth.json";
import enOverview from "./locales/en/overview.json";
import hrOverview from "./locales/hr/overview.json";
import enCharacter from "./locales/en/character.json";
import hrCharacter from "./locales/hr/character.json";

i18n.use(LanguageDetector)
    .use(initReactI18next)
    .init({
        resources: {
            en: {
                common: enCommon,
                auth: enAuth,
                overview: enOverview,
                character: enCharacter,
            },
            hr: {
                common: hrCommon,
                auth: hrAuth,
                overview: hrOverview,
                character: hrCharacter,
            },
        },
        fallbackLng: "en",
        ns: ["common", "auth", "overview", "character"], //namespaces
        defaultNS: "common",
        interpolation: {
            escapeValue: false,
        },
        detection: {
            order: ["localStorage", "navigator", "htmlTag"],
            caches: ["localStorage"],
        },
    });

export default i18n;
