 using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
{
    float healthPoints = -1;
    private bool isDead = false;
    Animator animator;
    ActionScheduler actionScheduler;
    [SerializeField] float RegenerationPercentage = 100f;
    
    private void Awake() 
    {
        animator =  GetComponent<Animator>();
        actionScheduler = GetComponent<ActionScheduler>();
        
    }
    private void Start() 
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth; //Level atlanıldıgında Healthı restore et
        if(healthPoints < 0)
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health); // Bu şekilde baslangıcta health ı BaseStattan cekecegiz.
        } //Cünkü diğer türlü karakter ölse bile start  RestoreState den once yüklendiği icin kayıt edip actıgımızda ölen karakter kayıt acılımında start ile canı tekrar yüklenip canlandırıyor.
    }     //Böylelikle bug olmamasını sağlıyoruz.

       

        public bool IsDead()
    {
        return isDead;
    }



    public void TakeDamage(GameObject İnstigator, float damage) //Buradaki instigator saldıran anlamında kullandık bu şekilde exp kazanacak karakteri belirleyeceğiz.
    {
        print(gameObject.name + "Took damage" + damage);
        healthPoints = Mathf.Max(healthPoints - damage,0); // Mathf max ile 0 dan assagı düsmesini engelledik.
            SoundManager.instance.bowSound();
            if (healthPoints == 0)
        {
            Die();
            AwardExperience(İnstigator);
        }
    }

    public float GetHealthPoints()
    {
        return healthPoints;
    }   

    public float GetMaxHealthPoints()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

        public float GetHealthPercentage()
    {
        return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health)); // Mevcut health ın yüzdesini al.
    }

    private void Die()
    {   
        if(isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        SoundManager.instance.deathSound();
        actionScheduler.CancelCurrentAction();//Eger karakter öldü ise tüm aksiyonları durdur.                   
    }
    private void AwardExperience(GameObject İnstigator) //Karakterin Experience kazanması
    {
        Experience experience = İnstigator.GetComponent<Experience>(); // Karakterin experience companenti varmı kontrol et
        if(experience == null) return;

        experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward)); //Experience gainExperienceReward kadar puan ekle.
    }
     private void RegenerateHealth()
    {
        float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (RegenerationPercentage / 100); //RegenarationPercentage kadar hp restore et
        healthPoints = Mathf.Max(healthPoints, regenHealthPoints); // Mathf.max ile verilen iki değerden en yüksek olanı sececek.
    }  //Yani healthpoints zaten yüksekse onunla devam edecek ancak regenHealthPoint daha büyük ise yani restore edilecek health mevcut healthden büyük ise restore edilecek canı sececek.
        public object CaptureState()
        {
            return healthPoints;
        }
        public void RestoreState(object state)
        {
        healthPoints = (float)state;
        if(healthPoints == 0)
        {
            Die();
            
        }
        }
}

}
