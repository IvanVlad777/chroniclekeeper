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
import enLocation from "./locales/en/location.json";
import hrLocation from "./locales/hr/location.json";
import enFaction from "./locales/en/faction.json";
import hrFaction from "./locales/hr/faction.json";
import enSpecies from "./locales/en/species.json";
import hrSpecies from "./locales/hr/species.json";
import enSocialClass from "./locales/en/socialClass.json";
import hrSocialClass from "./locales/hr/socialClass.json";
import enTimeline from "./locales/en/timeline.json";
import hrTimeline from "./locales/hr/timeline.json";
import enTag from "./locales/en/tag.json";
import hrTag from "./locales/hr/tag.json";
import enNote from "./locales/en/note.json";
import hrNote from "./locales/hr/note.json";

i18n.use(LanguageDetector)
    .use(initReactI18next)
    .init({
        resources: {
            en: {
                common: enCommon,
                auth: enAuth,
                overview: enOverview,
                character: enCharacter,
                location: enLocation,
                faction: enFaction,
                species: enSpecies,
                socialClass: enSocialClass,
                timeline: enTimeline,
                tag: enTag,
                note: enNote,
            },
            hr: {
                common: hrCommon,
                auth: hrAuth,
                overview: hrOverview,
                character: hrCharacter,
                location: hrLocation,
                faction: hrFaction,
                species: hrSpecies,
                socialClass: hrSocialClass,
                timeline: hrTimeline,
                tag: hrTag,
                note: hrNote,
            },
        },
        fallbackLng: "en",
        ns: [
            "common",
            "auth",
            "overview",
            "character",
            "location",
            "faction",
            "species",
            "socialClass",
            "timeline",
            "tag",
            "note",
        ], //namespaces
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
