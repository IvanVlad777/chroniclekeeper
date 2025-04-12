namespace ChronicleKeeper.Core.Enums
{
    public class SocietyEnums
    {
        public enum ElectionSystem
        {
            DirectElection,       // People vote directly
            Parliamentary,        // Elected representatives vote
            Hereditary,           // Passed down through lineage
            Meritocratic,         // Based on skills or achievements
            DivineMandate,        // Chosen by religious decree
            Oligarchic,           // Controlled by elites
            NoElections           // Absolute rule, no elections
        }

        public enum ScaleLevel
        {
            Low,      // Frequent uprisings, instability
            Moderate, // Some internal struggles, but functional
            High      // Strong and stable
        }

        public enum PunishmentMethods
        {
            DeathPenalty,
            Imprisonment,
            ForcedLabor,
            Fines,
            Exile,
            CorporalPunishment,
            Rehabilitation
        }

        public enum XenophobiaLevel
        {
            VeryOpen = 1,   // Welcoming to all species/nations
            Open = 2,       // Generally tolerant
            Neutral = 3,    // Neither welcoming nor hostile
            Suspicious = 4, // Wary of outsiders
            Extreme = 5     // Hostile or isolationist
        }

        public enum TechnologicalLevel
        {
            Primitive,       // Stone-age, no advanced tools
            Medieval,        // Swords, armor, castles
            Industrial,      // Steam power, early machinery
            Modern,          // Present-day tech
            Advanced,        // Beyond modern (AI, space travel)
            Futuristic,      // Highly advanced (intergalactic travel, nanotechnology)
            PostHuman        // Technology beyond biological limitations
        }

        public enum RankLevel
        {
            Master = 1,
            Journeyman = 2,
            Apprentice = 3
        }

        public enum FactionType
        {
            CriminalSyndicate = 1,    // Underground organizations (e.g., mafia, smugglers)
            ReligiousSect = 2,        // Unofficial religious groups (e.g., cults, heretical sects)
            PoliticalMovement = 3,    // Ideological groups pushing for change (e.g., rebels, revolutionaries)
            MercenaryCompany = 4,     // Private military groups (e.g., soldier-for-hire bands)
            TradeConsortium = 5,      // Large merchant organizations controlling trade
            ResistanceGroup = 6,      // Active opposition to a ruling power
            SecretSociety = 7,        // Shadow organizations with hidden agendas
            KnightlyOrder = 8,        // Chivalric or martial religious groups
            ScholarSociety = 9,       // Knowledge-focused groups (e.g., magic circles, philosopher guilds)
            ArcaneCoven = 10,         // Magical groups specializing in esoteric practices
            TechnologicalUnion = 11,  // Innovators, scientists, and engineers forming a tech-focused group
            MilitaryAlliance = 12,    // Coalition of armed forces, independent of national militaries
            PirateBrotherhood = 13,   // Bands of seafarers engaging in piracy and smuggling
            DiplomaticLeague = 14,    // A faction focused on diplomacy, peace treaties, and alliances
            RadicalExtremist = 15,    // Any faction with extreme ideological or violent tendencies
            Adventurers = 16          // A group of independent explorers and mercenaries
        }
    }
}
