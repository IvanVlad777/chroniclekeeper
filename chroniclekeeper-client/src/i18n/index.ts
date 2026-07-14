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
import enProfession from "./locales/en/profession.json";
import hrProfession from "./locales/hr/profession.json";
import enSchool from "./locales/en/school.json";
import hrSchool from "./locales/hr/school.json";
import enUniversity from "./locales/en/university.json";
import hrUniversity from "./locales/hr/university.json";
import enEducationSystem from "./locales/en/educationSystem.json";
import hrEducationSystem from "./locales/hr/educationSystem.json";
import enLibrary from "./locales/en/library.json";
import hrLibrary from "./locales/hr/library.json";
import enAbility from "./locales/en/ability.json";
import hrAbility from "./locales/hr/ability.json";
import enItem from "./locales/en/item.json";
import hrItem from "./locales/hr/item.json";
import enHistory from "./locales/en/history.json";
import hrHistory from "./locales/hr/history.json";
import enContent from "./locales/en/content.json";
import hrContent from "./locales/hr/content.json";
import enClimateZone from "./locales/en/climateZone.json";
import hrClimateZone from "./locales/hr/climateZone.json";
import enClimateDetail from "./locales/en/climateDetail.json";
import hrClimateDetail from "./locales/hr/climateDetail.json";
import enSeason from "./locales/en/season.json";
import hrSeason from "./locales/hr/season.json";
import enCreature from "./locales/en/creature.json";
import hrCreature from "./locales/hr/creature.json";
import enEconomicSystem from "./locales/en/economicSystem.json";
import hrEconomicSystem from "./locales/hr/economicSystem.json";
import enCurrency from "./locales/en/currency.json";
import hrCurrency from "./locales/hr/currency.json";
import enBankingSystem from "./locales/en/bankingSystem.json";
import hrBankingSystem from "./locales/hr/bankingSystem.json";
import enTaxationSystem from "./locales/en/taxationSystem.json";
import hrTaxationSystem from "./locales/hr/taxationSystem.json";
import enIndustry from "./locales/en/industry.json";
import hrIndustry from "./locales/hr/industry.json";
import enExtractionMethod from "./locales/en/extractionMethod.json";
import hrExtractionMethod from "./locales/hr/extractionMethod.json";
import enNaturalResource from "./locales/en/naturalResource.json";
import hrNaturalResource from "./locales/hr/naturalResource.json";
import enTradeRoute from "./locales/en/tradeRoute.json";
import hrTradeRoute from "./locales/hr/tradeRoute.json";
import enGuild from "./locales/en/guild.json";
import hrGuild from "./locales/hr/guild.json";
import enCorporation from "./locales/en/corporation.json";
import hrCorporation from "./locales/hr/corporation.json";

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
                profession: enProfession,
                school: enSchool,
                university: enUniversity,
                educationSystem: enEducationSystem,
                library: enLibrary,
                ability: enAbility,
                item: enItem,
                history: enHistory,
                content: enContent,
                climateZone: enClimateZone,
                climateDetail: enClimateDetail,
                season: enSeason,
                creature: enCreature,
                economicSystem: enEconomicSystem,
                currency: enCurrency,
                bankingSystem: enBankingSystem,
                taxationSystem: enTaxationSystem,
                industry: enIndustry,
                extractionMethod: enExtractionMethod,
                naturalResource: enNaturalResource,
                tradeRoute: enTradeRoute,
                guild: enGuild,
                corporation: enCorporation,
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
                profession: hrProfession,
                school: hrSchool,
                university: hrUniversity,
                educationSystem: hrEducationSystem,
                library: hrLibrary,
                ability: hrAbility,
                item: hrItem,
                history: hrHistory,
                content: hrContent,
                climateZone: hrClimateZone,
                climateDetail: hrClimateDetail,
                season: hrSeason,
                creature: hrCreature,
                economicSystem: hrEconomicSystem,
                currency: hrCurrency,
                bankingSystem: hrBankingSystem,
                taxationSystem: hrTaxationSystem,
                industry: hrIndustry,
                extractionMethod: hrExtractionMethod,
                naturalResource: hrNaturalResource,
                tradeRoute: hrTradeRoute,
                guild: hrGuild,
                corporation: hrCorporation,
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
            "profession",
            "school",
            "university",
            "educationSystem",
            "library",
            "ability",
            "item",
            "history",
            "content",
            "climateZone",
            "climateDetail",
            "season",
            "creature",
            "economicSystem",
            "currency",
            "bankingSystem",
            "taxationSystem",
            "industry",
            "extractionMethod",
            "naturalResource",
            "tradeRoute",
            "guild",
            "corporation",
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
