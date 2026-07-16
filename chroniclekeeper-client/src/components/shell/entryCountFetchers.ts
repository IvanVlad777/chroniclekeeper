/**
 * Maps a nav entry key to a world-scoped list fetcher returning the entry's
 * items. The domain panel uses these lazily (active domain only) to show a
 * count per entry — count = array length; there are no dedicated count
 * endpoints. Entries without a fetcher here simply render no count.
 */
import { getCharacters } from "../../api/characters";
import { getLocations } from "../../api/locations";
import { getFactions } from "../../api/factions";
import { getSpecies } from "../../api/species";
import { getSocialClasses } from "../../api/socialClasses";
import { getNations } from "../../api/nations";
import { getReligions } from "../../api/religions";
import { getLanguages } from "../../api/languages";
import { getCultures } from "../../api/cultures";
import { getTimelines } from "../../api/timelines";
import { getTags } from "../../api/tags";
import { getNotes } from "../../api/notes";
import { getPoliticalIdeologies } from "../../api/politicalIdeologies";
import { getGovernmentSystems } from "../../api/governmentSystems";
import { getPoliticalParties } from "../../api/politicalParties";
import { getLegalSystems } from "../../api/legalSystems";
import { getDiplomaticAgreements } from "../../api/diplomaticAgreements";
import { getProfessions } from "../../api/professions";
import { getSchools } from "../../api/schools";
import { getUniversities } from "../../api/universities";
import { getEducationSystems } from "../../api/educationSystems";
import { getLibraries } from "../../api/libraries";
import { getAbilities } from "../../api/abilities";
import { getItems } from "../../api/items";
import { getHistories } from "../../api/histories";
import { getContents } from "../../api/contents";
import { getClimateZones } from "../../api/climateZones";
import { getClimateDetails } from "../../api/climateDetails";
import { getSeasons } from "../../api/seasons";
import { getCreatures } from "../../api/creatures";
import { getEconomicSystems } from "../../api/economicSystems";
import { getCurrencies } from "../../api/currencies";
import { getBankingSystems } from "../../api/bankingSystems";
import { getTaxationSystems } from "../../api/taxationSystems";
import { getIndustries } from "../../api/industries";
import { getExtractionMethods } from "../../api/extractionMethods";
import { getNaturalResources } from "../../api/naturalResources";
import { getTradeRoutes } from "../../api/tradeRoutes";
import { getGuilds } from "../../api/guilds";
import { getCorporations } from "../../api/corporations";

export type CountFetcher = (worldId: number) => Promise<unknown[]>;

export const entryCountFetchers: Record<string, CountFetcher> = {
    characters: (w) => getCharacters(w),
    locations: (w) => getLocations(w),
    factions: (w) => getFactions(w),
    species: (w) => getSpecies(w),
    socialClasses: (w) => getSocialClasses(w),
    nations: (w) => getNations(w),
    religions: (w) => getReligions(w),
    languages: (w) => getLanguages(w),
    cultures: (w) => getCultures(w),
    timelines: (w) => getTimelines(w),
    tags: (w) => getTags(w),
    notes: (w) => getNotes(w),
    politicalIdeologies: (w) => getPoliticalIdeologies(w),
    governmentSystems: (w) => getGovernmentSystems(w),
    politicalParties: (w) => getPoliticalParties(w),
    legalSystems: (w) => getLegalSystems(w),
    diplomaticAgreements: (w) => getDiplomaticAgreements(w),
    professions: (w) => getProfessions(w),
    schools: (w) => getSchools({ worldId: w }),
    universities: (w) => getUniversities({ worldId: w }),
    educationSystems: (w) => getEducationSystems(w),
    libraries: (w) => getLibraries(w),
    abilities: (w) => getAbilities(w),
    items: (w) => getItems({ worldId: w }),
    histories: (w) => getHistories(w),
    contents: (w) => getContents({ worldId: w }),
    climateZones: (w) => getClimateZones(w),
    climateDetails: (w) => getClimateDetails(w),
    seasons: (w) => getSeasons(w),
    creatures: (w) => getCreatures({ worldId: w }),
    economicSystems: (w) => getEconomicSystems(w),
    currencies: (w) => getCurrencies(w),
    bankingSystems: (w) => getBankingSystems(w),
    taxationSystems: (w) => getTaxationSystems(w),
    industries: (w) => getIndustries(w),
    extractionMethods: (w) => getExtractionMethods(w),
    naturalResources: (w) => getNaturalResources(w),
    tradeRoutes: (w) => getTradeRoutes(w),
    guilds: (w) => getGuilds(w),
    corporations: (w) => getCorporations(w),
    // overview has no list — intentionally absent (no count shown).
};
