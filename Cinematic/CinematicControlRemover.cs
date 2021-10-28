using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;


namespace RPG.Cinematics
{
    // Observe Pattern
    public class CinematicControlRemover : MonoBehaviour
{
   GameObject player;
    void Start()
    {
        GetComponent<PlayableDirector>().played += DisableControl; // Eğer playebleDirectorten played oynatılırsa  DisableControl fonksiyonunuda calıstır.Yani cinematic oynatılırsa kontrolu etkisiz bırak.
        GetComponent<PlayableDirector>().stopped += EnableControl; // Eğer playebleDirectorten played oynatılırsa  EnableControl fonksiyonunuda calıstır.Yani cinematic biterse kontrolu etkinlestir.
        player = GameObject.FindWithTag("Player");
    }  // GetComponent<PlayableDirector>(). sonra yıldırım işareti olanları kullanarak observe pattern kullanabiliriz.

    
   void DisableControl(PlayableDirector pd) // pd yazdık cünkü bu fonksiyonu kullanmayacağız sadece boşlugu doldurmak icin yazdık.
   {                                        // Cünkü zaten DisableControl yukarıda cagırdık.
      
      player.GetComponent<ActionScheduler>().CancelCurrentAction();
      player.GetComponent<PlayerController>().enabled = false;
   }
   void EnableControl(PlayableDirector pd)
   {
        player.GetComponent<PlayerController>().enabled = true;
   }
}
}

