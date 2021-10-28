using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
public class HealthDisplay : MonoBehaviour
{
   Health health;

   private void Awake()
   {
       health = GameObject.FindWithTag("Player").GetComponent<Health>();
   }
   private void Update()
   {
       GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(),health.GetMaxHealthPoints());
   }   //String.Format methoduyla ilk sıfır ile ulaşılan ilk objeyi yani health.GetHealthPercentage()) yazacak
       //İkinci sıfır ise % yi tam sayı olarak yenileyecek 99.5 göstermezken 99 98 olarak tek tek ondalıklar olmadan gösterecek
       //Son olarak {0:0} yanındaki % ise health.GetHealthPercentage()) hemen yanında yazılacak.
}

}
