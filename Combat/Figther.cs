using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Figther : MonoBehaviour, IAction, ISaveable, IModifierProvider
{   
    Health target;
    
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] Transform RightHandTransform = null;
    [SerializeField] Transform LeftHandTransform = null;
    [SerializeField] Weapon defaultWeapon = null;
    
    
    

    float timeSinceLastAttack = Mathf.Infinity;

    Weapon currentWeapon = null;
    private void Start() 
    {
        if(currentWeapon == null) // Eger kayıttaki silah boş olsa bile korumaya aldık default weapon giyicek
        {
            EquipWeapon(defaultWeapon); // 
        }
        
    }                      

        private void Update()
        {
            
            timeSinceLastAttack += Time.deltaTime;
            if(target == null) return; //Düsmana tıklanmadı ise update calıstırma
            if(target.IsDead()) return; // Düsman öldü ise atak durdur.

            if (!IsInRange()) // Düsmana tıklandı fakat yakında değilse oraya yürü
            {
                GetComponent<Move>().moveTo(target.transform.position,1f); //1f düsmana saldırırken full hızda kos demek.
            }
            else
            {
                GetComponent<Move>().Cancel(); // Düsmana yakın ve tıklanmıssa dur
                AttackTrigger();      
            }          

        }
    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
       
        Animator animator = GetComponent<Animator>();
        weapon.Spawn(RightHandTransform,LeftHandTransform, animator);//Silahı giydiğinde attack animasyonunu değiştirdik.
    }
    public Health GetTarget()
    {
        return target;
    }
    public bool canAttack(GameObject combatTarget) //düsmanın olunce etkisiz kalması(Tıklanamaz hale getirme.)
    {
       if(combatTarget == null) { return false; } // Eger karakter dısında bir yere tıklanırsa saldırı yapılamaz
       Health targetToAttack = combatTarget.GetComponent<Health>(); 
       return targetToAttack != null && !targetToAttack.IsDead(); //Ancak karaktere tıklarsan ve ölü değilse saldır
    }
   
    private void AttackTrigger()
    {
        transform.LookAt(target.transform);
        if(timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerToAttack();
                timeSinceLastAttack = 0f;
                
            }
        }

        private void TriggerToAttack()
        {
            GetComponent<Animator>().ResetTrigger("CancelAttack"); //Atağa başlamadan once cancelattack trigger resetle ki glitch olmasın
            GetComponent<Animator>().SetTrigger("Attack");
        }

        //Animation Event
    void Hit()
    {   
        if(target == null) { return; }

        float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
        if(currentWeapon.HasProjectile())
        {
            currentWeapon.LaunchProjectile(RightHandTransform,LeftHandTransform,target,gameObject,damage);
                   
        }
        else
        {
            target.TakeDamage(gameObject,damage); 
        }
         
    }
    
    private bool IsInRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
    }

    public void Attack(GameObject combatTarget)
    {
        GetComponent<ActionScheduler>().startAction(this);
        target = combatTarget.GetComponent<Health>();
    }
    public void Cancel()
    {
        stopAttack();
        target = null;
        GetComponent<Move>().Cancel();
    }
    private void stopAttack()
    {
        GetComponent<Animator>().SetTrigger("CancelAttack");
        GetComponent<Animator>().ResetTrigger("Attack");
    }

     public IEnumerable<float> GetadditiveModifier(Stat stat)
    {
        if(stat == Stat.Damage)
        {
            yield return currentWeapon.GetDamage();
        }
    }

    public object CaptureState()
    {
        return currentWeapon.name; // current weapon ismini kaydet
    }

    public void RestoreState(object state)
    {
        string weaponName = (string)state;
        Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName); // Oyuna baslayınca Resource icindeki silahlardan defaultWEaponName e atadıgımızı sececek ve yükleyecek.
        EquipWeapon(weapon); // UnityEngine.Resources yazdık cünkü diğer türlü namespace olan Resources ile cakısıyordu.
        
    }

       
    }

    
}
