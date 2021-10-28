using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
   [SerializeField] AnimatorOverrideController animatorOverride = null;
   [SerializeField] GameObject EquippedWeapon = null; 
   [SerializeField]float weaponDamage = 5f;
   [SerializeField] float weaponRange = 2f;
   [SerializeField] bool isRightHanded = true;
   [SerializeField] Projectile projectile = null;

   const string weaponName = "Weapon";

   public void Spawn(Transform RightHand, Transform LeftHand, Animator animator)
   {
        DestroyOldWeapon(RightHand,LeftHand); //silah spawn olurken onceden elde bir silah varsa yok et

        if(EquippedWeapon != null)
        {
            Transform HandTransform = GetTransform(RightHand, LeftHand);//Hangi silahın Hangi elde Spawn olacagını seceriz.
            GameObject weapon = Instantiate(EquippedWeapon, HandTransform);
            weapon.name = weaponName;
        }
        
        var overrideController = animator.runtimeAnimatorController  as AnimatorOverrideController; //overrideController eğer mevcut animator controlleri yalnızca main controllera dönüsmüs ise null dönecek.
        //runtimeanimator ana animator controlleri override ise gecici olarak eklediğimiz animator controlleri.
        if (animatorOverride != null)
        {
            animator.runtimeAnimatorController = animatorOverride; //main controlleri override controlera cevir.
        }
        else if(overrideController != null)//Eger animator controllerin override controllera gecmesi gerekirken editorde boş bırakılmıs işe default olarak main controller kullan
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController; //default olarak main controllera dön
        }// Yani özet olarak eger olurda farklı silahların animator override controllerlarını koymayı unutursak default olarak main animatoru kullan
          
   }
   private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
   {
       Transform oldWeapon = rightHand.Find(weaponName); //default silahı sağ elde
       if(oldWeapon == null)
       {
           oldWeapon = leftHand.Find(weaponName); //Silah sağ elde değilse sol eldedir.
       }
       if(oldWeapon == null) return; // Eger silah sol eldede yok ise iki elde boştur ve bu kodu calıstırma
       
       oldWeapon.name = "DESTROYİNG"; // sadece eski silahı silmesi icin eski silahın adını değiştirdik.Yoksa yeni silahıda silecekti.
       Destroy(oldWeapon.gameObject); // Eger iki elden birinde silah varsa yok et

   }

        private Transform GetTransform(Transform RightHand, Transform LeftHand)
        {
            Transform HandTransform;
            if (isRightHanded) HandTransform = RightHand;
            else HandTransform = LeftHand;
            return HandTransform;
        }

        public bool HasProjectile()
   {
       return projectile != null;
   }

   public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target,GameObject instigator, float calculatedDamage) //  Ateşli ve yaylı silahlardaki mermi
   {
       Projectile projectileİnstance = Instantiate(projectile,GetTransform(leftHand,rightHand).position,Quaternion.identity); //Quaternion identity objenin rotasyonunu alır.
       projectileİnstance.SetTarget(target,instigator,calculatedDamage);
   }

   public float GetDamage()
   {
       return weaponDamage;
   }
   public float GetRange()
   {
       return weaponRange;
   }
}

}