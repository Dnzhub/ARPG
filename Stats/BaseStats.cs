using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject LevelUpParticleEffect = null;

        int currentLevel = 0;
        public event Action onLevelUp; // Level atlayınca calısacak.
        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel; // Sadece experience kazanıldıgında Updatelevel fonksiyonu calısacak
            }                                                 // Buda kod optimizasyonunda yardımcı olacak.
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
                //print("Level Up !")
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(LevelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat,characterClass, GetLevel()) + GetadditiveModifier(stat);
        }

        private float GetadditiveModifier(Stat stat)
        {
            float total = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetadditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total; 
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel(); // current level olurda 0 a düserse yani bi bug olursa düzelmek icin.
            }
            return currentLevel; 

        }
        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if(experience == null)
            {
                return startingLevel; // Eger hedefte experience özelliği yok ise hesaplama yapma yani aşşağıdaki kodları calıstırma.
            }


            float currentXP = experience.GetExperience();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp,characterClass); // Son levelden bir önceki leveli al
            for (int level = 1; level <= penultimateLevel; level++)  //sondan bir önceki level(penultimatelevel) a eşit yada kücük ise mevcut levelimiz aşşağıdaki işlemi yap.
            {
                float XPtoLevelUp = progression.GetStat(Stat.ExperienceToLevelUp,characterClass,level); //Kazanılacak xp
                if(XPtoLevelUp > currentXP)
                {
                    return level; // eger kazanılan xp mevcut xp den büyük ise kazanılana uygun leveli return et
                }
            }
            return penultimateLevel + 1; // eğer son levelden bir oncesine gelindiyse ve level alınacaksa son levela +1 level ekle ve max levele gelmiş ol.
        }

    }
} 

