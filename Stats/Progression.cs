using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }
        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length; // Length return etmemizin sebebi son levelin exp puanını gecince son levelde kalmamızı hesap etmek icin.
        }

        private void BuildLookup() //Optimizasyon icin bu kod sürekli calısmayacak bir kere kurulacak ve lazım oldugunda tekrar kurulacak.Dictionaryler ile
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>(); //Character class ,stat ve levellerı iceren bi dictionary olustur

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>(); // Aynı şekilde stat ve levellerı iceren dictionary olustur. Float sebebi level int ten olusuyor ve int yerleşik bi değer oldugu icin dictionarye direk float olarak gecirebiliyoruz.Ayrıyetten class tanımlamasına gerek yok.

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                
                    statLookupTable[progressionStat.stat] = progressionStat.levels; //Statların altındaki levellere ulas
                }

                lookupTable[progressionClass.characterClass] = statLookupTable; // Classların altındaki statlara ulas
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}