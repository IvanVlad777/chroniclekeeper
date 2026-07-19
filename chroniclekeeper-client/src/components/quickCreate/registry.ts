import { createLocation } from '../../api/locations';
import { createCharacter } from '../../api/characters';
import { createFaction } from '../../api/factions';
import { createSpecies, createRace } from '../../api/species';
import { createGovernmentSystem } from '../../api/governmentSystems';
import { createLegalSystem } from '../../api/legalSystems';
import { createEducationSystem } from '../../api/educationSystems';
import { createEconomicSystem } from '../../api/economicSystems';
import { createMilitaryDoctrine } from '../../api/militaryDoctrines';
import { createMilitaryOrganization } from '../../api/militaryOrganizations';
import { createSocialHierarchy } from '../../api/socialHierarchies';
import { createSocialClass } from '../../api/socialClasses';
import { createNation } from '../../api/nations';
import { createReligion } from '../../api/religions';
import { createLanguage } from '../../api/languages';
import { createProfession } from '../../api/professions';
import { createCurrency } from '../../api/currencies';
import { createContent } from '../../api/contents';
import { createBankingSystem } from '../../api/bankingSystems';
import { createTaxationSystem } from '../../api/taxationSystems';
import { createIndustry } from '../../api/industries';
import { createCreature } from '../../api/creatures';
import { createPoliticalIdeology } from '../../api/politicalIdeologies';
import { createExtractionMethod } from '../../api/extractionMethods';
import { createUniversity } from '../../api/universities';
import { createHistory } from '../../api/histories';
import { createCorporation } from '../../api/corporations';

/** Minimal result the picker needs back from a quick-create. */
export interface QuickCreated {
  id: number;
  name: string;
}

/** The two fields the quick-create modal collects. */
export interface QuickCreateInput {
  name: string;
  description: string;
}

/**
 * Entity-specific context a picker can hand to a descriptor for required
 * relations that a name/description modal can't collect (e.g. a race needs
 * its parent species). Extend as more kinds get quick-create.
 */
export interface QuickCreateContext {
  sapientSpeciesId?: number;
  /** parent education system — a University derives its world from it. */
  educationSystemId?: number;
}

interface QuickCreateDescriptor {
  /** false when the entity's CreateDto has no Description field. */
  description?: boolean;
  /** whether this kind needs a non-empty context to create. */
  requiresContext?: (ctx: QuickCreateContext | undefined) => boolean;
  create: (
    worldId: number,
    input: QuickCreateInput,
    ctx?: QuickCreateContext,
  ) => Promise<QuickCreated>;
}

/**
 * One place mapping an entity-picker "kind" to how it quick-creates that
 * entity with sensible defaults for the required fields a two-field modal
 * doesn't ask about. The user refines the rest on the entity's own page.
 */
const registry = {
  location: {
    create: (worldId, { name, description }) =>
      createLocation({ name, description, worldId, type: 'Other' }),
  },
  character: {
    create: (worldId, { name, description }) =>
      createCharacter({
        name,
        firstName: name,
        lastName: '',
        nickname: '',
        title: '',
        birthDate: null,
        deathDate: null,
        description,
        height: null,
        weight: null,
        hairColor: '',
        eyeColor: '',
        specialPhysicalFeatures: '',
        isArtificial: false,
        worldId,
        sapientSpeciesId: null,
        raceId: null,
        socialClassId: null,
        nationId: null,
        religionId: null,
        professionId: null,
        historyId: null,
        fatherId: null,
        motherId: null,
      }),
  },
  faction: {
    create: (worldId, { name, description }) =>
      createFaction({
        name,
        description,
        worldId,
        type: 'Adventurers',
        isSecretive: false,
        motto: '',
      }),
  },
  species: {
    create: (worldId, { name, description }) =>
      createSpecies({
        name,
        description,
        worldId,
        commonName: '',
        scientificName: '',
        isHumanoid: true,
        lifespan: '',
        sapientType: 'Humanoid',
        averageLifespan: 0,
        height: 0,
        weight: 0,
        isSentient: true,
        isArtificial: false,
        artificialOrigin: null,
        parentCreatureId: null,
        historyId: null,
      }),
  },
  race: {
    // Race derives its world from the parent species — needs it in context.
    requiresContext: (ctx) => !ctx?.sapientSpeciesId,
    create: (_worldId, { name, description }, ctx) =>
      createRace({
        name,
        description,
        sapientSpeciesId: ctx?.sapientSpeciesId ?? 0,
        appearanceTraits: '',
        geneticFeatures: '',
        adaptations: '',
      }),
  },
  governmentSystem: {
    create: (worldId, { name, description }) =>
      createGovernmentSystem({
        name,
        description,
        worldId,
        isDemocratic: false,
        isMonarchic: false,
        isReligious: false,
        isFederal: false,
        isCentralized: false,
        politicalIdeologyId: null,
        electionSystem: 'NoElections',
        stabilityLevel: 'Moderate',
        hasTermLimits: false,
        maxTermLength: null,
      }),
  },
  legalSystem: {
    create: (worldId, { name, description }) =>
      createLegalSystem({
        name,
        description,
        worldId,
        laws: '',
        judicialIndependence: 'Moderate',
        punishmentMethods: 'Imprisonment',
      }),
  },
  educationSystem: {
    create: (worldId, { name, description }) =>
      createEducationSystem({
        name,
        description,
        worldId,
        isStateControlled: false,
        allowsPrivateInstitutions: false,
        includesReligiousEducation: false,
        supportsGuildTraining: false,
      }),
  },
  economicSystem: {
    create: (worldId, { name, description }) =>
      createEconomicSystem({
        name,
        description,
        worldId,
        isMarketDriven: false,
        hasStateControl: false,
        isFeudal: false,
        allowsCorporations: false,
        allowsGuilds: false,
        taxationSystemId: null,
        bankingSystemId: null,
        historyId: null,
      }),
  },
  militaryDoctrine: {
    create: (worldId, { name, description }) =>
      createMilitaryDoctrine({
        name,
        description,
        worldId,
        historyId: null,
        strategy: '',
        philosophy: '',
        prioritizesInfantry: false,
        prioritizesCavalry: false,
        prioritizesArtillery: false,
        prioritizesNavalForces: false,
        prioritizesAirForces: false,
        requiresHeavyIndustry: false,
        usesMercenaries: false,
      }),
  },
  militaryOrganization: {
    create: (worldId, { name, description }) =>
      createMilitaryOrganization({
        name,
        description,
        worldId,
        historyId: null,
        type: '',
        role: '',
        militaryDoctrineId: null,
      }),
  },
  socialHierarchy: {
    create: (worldId, { name, description }) =>
      createSocialHierarchy({
        name,
        description,
        worldId,
        isCasteSystem: false,
        allowsUpwardMobility: false,
        allowsIntermarriage: false,
        enforcesLegalSeparation: false,
        historyId: null,
      }),
  },
  socialClass: {
    create: (worldId, { name, description }) =>
      createSocialClass({
        name,
        description,
        worldId,
        isNoble: false,
        isMerchantClass: false,
        isOutcast: false,
        canOwnLand: false,
        canHoldOffice: false,
        hasTaxExemptions: false,
        socialHierarchyId: null,
      }),
  },
  nation: {
    create: (worldId, { name, description }) =>
      createNation({
        name,
        description,
        worldId,
        population: 0,
        socialHierarchyId: null,
        historyId: null,
      }),
  },
  religion: {
    create: (worldId, { name, description }) =>
      createReligion({
        name,
        description,
        worldId,
        coreBeliefs: '',
        practices: '',
        hasDeities: false,
        isStateReligion: false,
      }),
  },
  language: {
    create: (worldId, { name, description }) =>
      createLanguage({
        name,
        description,
        worldId,
        writingSystem: '',
        isExtinct: false,
        dialects: '',
      }),
  },
  profession: {
    create: (worldId, { name, description }) =>
      createProfession({
        name,
        description,
        worldId,
        requiredSkills: '',
        workEnvironment: '',
      }),
  },
  currency: {
    create: (worldId, { name, description }) =>
      createCurrency({
        name,
        description,
        worldId,
        symbol: '',
        exchangeRate: 0,
        backingType: '',
        historyId: null,
      }),
  },
  content: {
    create: (worldId, { name, description }) =>
      createContent({
        name,
        description,
        worldId,
        type: 'Book',
      }),
  },
  bankingSystem: {
    create: (worldId, { name, description }) =>
      createBankingSystem({
        name,
        description,
        worldId,
        systemType: '',
        interestRate: 0,
        allowsLoans: false,
        hasStateControl: false,
        supportsForeignInvestment: false,
        currencyId: null,
        historyId: null,
      }),
  },
  taxationSystem: {
    create: (worldId, { name, description }) =>
      createTaxationSystem({
        name,
        description,
        worldId,
        incomeTaxRate: 0,
        corporateTaxRate: 0,
        tradeTariffRate: 0,
        hasFlatTax: false,
        hasWealthTax: false,
        historyId: null,
      }),
  },
  industry: {
    create: (worldId, { name, description }) =>
      createIndustry({
        name,
        description,
        worldId,
        sector: '',
        employmentRate: 0,
        historyId: null,
      }),
  },
  creature: {
    create: (worldId, { name, description }) =>
      createCreature({
        name,
        description,
        worldId,
        // Subtype is required (non-empty) and must be a CreatureSubtype —
        // "Sapient" creatures are filtered out of the list, so default to Animal.
        subtype: 'Animal',
        type: 'Animal',
        averageLifespan: 0,
        height: 0,
        weight: 0,
        isSentient: false,
        isArtificial: false,
        artificialOrigin: null,
        parentCreatureId: null,
        historyId: null,
      }),
  },
  politicalIdeology: {
    create: (worldId, { name, description }) =>
      createPoliticalIdeology({
        name,
        description,
        worldId,
        isAuthoritarian: false,
        isSocialist: false,
        isLiberal: false,
        isRadical: false,
        isMilitaristic: false,
        supportsFreeMarket: false,
        supportsPlannedEconomy: false,
      }),
  },
  extractionMethod: {
    create: (worldId, { name, description }) =>
      createExtractionMethod({
        name,
        description,
        worldId,
        methodType: '',
        isSustainable: false,
        historyId: null,
      }),
  },
  history: {
    create: (worldId, { name, description }) =>
      createHistory({
        name,
        description,
        worldId,
        summary: '',
        isOfficial: false,
      }),
  },
  corporation: {
    create: (worldId, { name, description }) =>
      createCorporation({
        name,
        description,
        worldId,
        industrySector: '',
        revenue: 0,
        numberOfEmployees: 0,
        isPubliclyTraded: false,
        isStateOwned: false,
        industryId: null,
        taxationSystemId: null,
        bankingSystemId: null,
        parentCorporationId: null,
        historyId: null,
      }),
  },
  university: {
    // A University derives its world from its parent education system —
    // needs it in context (hidden add-row until one is chosen).
    requiresContext: (ctx) => !ctx?.educationSystemId,
    create: (_worldId, { name, description }, ctx) =>
      createUniversity({
        name,
        description,
        educationSystemId: ctx?.educationSystemId ?? 0,
        focusesOnScience: false,
        focusesOnMagic: false,
        focusesOnPhilosophy: false,
        focusesOnMilitaryStudies: false,
      }),
  },
} satisfies Record<string, QuickCreateDescriptor>;

export type QuickCreateKind = keyof typeof registry;

export const quickCreateRegistry: Record<QuickCreateKind, QuickCreateDescriptor> =
  registry;
