import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";

import enCommon from "./locales/en/common.json";
import hrCommon from "./locales/hr/common.json";
import enLogin from "./locales/en/login.json";
import hrLogin from "./locales/hr/login.json";
import enRegister from "./locales/en/register.json";
import hrRegister from "./locales/hr/register.json";
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
                login: enLogin,
                register: enRegister,
                overview: enOverview,
                character: enCharacter,
            },
            hr: {
                common: hrCommon,
                login: hrLogin,
                register: hrRegister,
                overview: hrOverview,
                character: hrCharacter,
            },
        },
        lng: "en",
        fallbackLng: "en",
        ns: ["login", "register", "overview", "character"], //namespaces
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
