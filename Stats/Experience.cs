using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
{
    [SerializeField] float experiencePoints = 0;   
                                            // Delegate ve eventler ile bir liste olusturup icinde fonksiyonlar tanımlayabiliriz. 
    public event Action onExperienceGained; // Action Delegateler gibi kullanılabilir tek farkı delegateler gibi bir değeri return etmesi gerekmez.
       

        public void GainExperience(float experience)
        {   
            experiencePoints += experience;  
            onExperienceGained();  
        }
        public float GetExperience()
        {
            return experiencePoints;
        }
         public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}

