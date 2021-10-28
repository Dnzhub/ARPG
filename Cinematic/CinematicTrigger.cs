using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.SceneManagement;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour , ISaveable
{
    private bool isTrigger = false;
    
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player" && isTrigger == false)
       {
           isTrigger = true;
           StartCoroutine(FadeToCamera());
       }
       
        
    }
    private IEnumerator FadeToCamera()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(0.5f);
        GetComponent<PlayableDirector>().Play(); // Cinematic animasyonunu oynat
        
        yield return fader.FadeIn(1f);
    }

        public object CaptureState()
        {
            return isTrigger;  // Eger Cinematic oynatılıp save edilmiş ise load edildiginde tekrar cinematic oynatılmayacak.
        }

        public void RestoreState(object state)
        {
            isTrigger = (bool)state;
        }
    }
}

