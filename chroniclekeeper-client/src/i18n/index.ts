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
import enNation from "./locales/en/nation.json";
import hrNation from "./locales/hr/nation.json";
import enReligion from "./locales/en/religion.json";
import hrReligion from "./locales/hr/religion.json";
import enLanguage from "./locales/en/language.json";
import hrLanguage from "./locales/hr/language.json";
import enCulture from "./locales/en/culture.json";
import hrCulture from "./locales/hr/culture.json";
import enTimeline from "./locales/en/timeline.json";
import hrTimeline from "./locales/hr/timeline.json";
import enTag from "./locales/en/tag.json";
import hrTag from "./locales/hr/tag.json";
import enNote from "./locales/en/note.json";
import hrNote from "./locales/hr/note.json";
import enPoliticalIdeology from "./locales/en/politicalIdeology.json";
import hrPoliticalIdeology from "./locales/hr/politicalIdeology.json";
import enGovernmentSystem from "./locales/en/governmentSystem.json";
import hrGovernmentSystem from "./locales/hr/governmentSystem.json";
import enPoliticalParty from "./locales/en/politicalParty.json";
import hrPoliticalParty from "./locales/hr/politicalParty.json";
import enLegalSystem from "./locales/en/legalSystem.json";
import hrLegalSystem from "./locales/hr/legalSystem.json";
import enDiplomaticAgreement from "./locales/en/diplomaticAgreement.json";
import hrDiplomaticAgreement from "./locales/hr/diplomaticAgreement.json";

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
                nation: enNation,
                religion: enReligion,
                language: enLanguage,
                culture: enCulture,
                timeline: enTimeline,
                tag: enTag,
                note: enNote,
                politicalIdeology: enPoliticalIdeology,
                governmentSystem: enGovernmentSystem,
                politicalParty: enPoliticalParty,
                legalSystem: enLegalSystem,
                diplomaticAgreement: enDiplomaticAgreement,
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
                nation: hrNation,
                religion: hrReligion,
                language: hrLanguage,
                culture: hrCulture,
                timeline: hrTimeline,
                tag: hrTag,
                note: hrNote,
                politicalIdeology: hrPoliticalIdeology,
                governmentSystem: hrGovernmentSystem,
                politicalParty: hrPoliticalParty,
                legalSystem: hrLegalSystem,
                diplomaticAgreement: hrDiplomaticAgreement,
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
            "nation",
            "religion",
            "language",
            "culture",
            "timeline",
            "tag",
            "note",
            "politicalIdeology",
            "governmentSystem",
            "politicalParty",
            "legalSystem",
            "diplomaticAgreement",
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
