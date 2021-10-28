using System;
using RPG.Resources;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Combat
{
   public class EnemyHealthDisplay : MonoBehaviour
{
   Figther figther;

   private void Awake()
   {
       figther = GameObject.FindWithTag("Player").GetComponent<Figther>(); //Figtherdan almamızın sebebi tıkladığımız hedefi secmek.
   }
   private void Update()
   {
       if(figther.GetTarget() == null)
       {
           GetComponent<Text>().text = "No Target"; //Eger tıklanmıs hedef yok ise No Target yaz
           return;
       }
       Health health = figther.GetTarget(); //Playerın tıkladığı hedefin health ini al
       GetComponent<Text>().text =String.Format("{0:0}/{1:0}", health. GetHealthPoints(),health.GetMaxHealthPoints());
   }   //String.Format methoduyla ilk sıfır ile ulaşılan ilk objeyi yani health.GetHealthPercentage()) yazacak
       //İkinci sıfır ise % yi tam sayı olarak yenileyecek 99.5 göstermezken 99 98 olarak tek tek ondalıklar olmadan gösterecek
       //Son olarak {0:0} yanındaki % ise health.GetHealthPercentage()) hemen yanında yazılacak.
}

}