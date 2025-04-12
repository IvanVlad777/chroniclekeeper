using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Enums
{
    public class CreatureEnums
    {
        public enum CreatureType
        {
            Sapient = 1,
            Animal = 2,
            Plant = 3,
            Fungus = 4,
            Spiritual = 5,
            Mythical = 6
        }

        public enum DietType
        {
            Herbivore = 1,
            Carnivore = 2,
            Omnivore = 3
        }

        public enum SapientType
        {
            Humanoid = 1, // Elves, Orcs, Dwarves, Humans
            Bestial = 2, // Beast-like, e.g., Minotaurs, Lizardfolk
            Elemental = 3, // Fire beings, Water elementals
            Celestial = 4, // Angels, divine beings
            Infernal = 5, // Demons, fiends
            Spirit = 6, // Ghosts, Ancestors, Trickster Spirits
            Deity = 7, // Gods and divine figures
            Otherworldly = 8 // Lovecraftian horrors, void creatures
        }

        public enum ArtificialOrigin
        {
            Robot = 1, // Fully mechanical being
            AI = 2, // Digital intelligence
            Cyborg = 3, // Part organic, part machine
            Golem = 4, // Magical construct
            Clone = 5, // Artificially created biological being
            Bioengineered = 6 // Genetically modified or lab-created life
        }
        public enum DeityType
        {
            Elemental,         // Gods of natural forces (fire, water, air, earth)
            Cosmic,            // Gods representing universal concepts (time, fate, death)
            Anthropomorphic,   // Human-like gods
            Animalistic,       // Gods depicted as animals
            Alien,             // Non-human, non-earthly divine beings
            Machine,           // Artificial or robotic deities
            Unknown            // Undefined or mysterious divine entities
        }
    }
}
