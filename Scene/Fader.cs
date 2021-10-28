using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {      
      canvasGroup = GetComponent<CanvasGroup>();
      
     
    }
    public void FadeOutİmmediate()
    {
        canvasGroup.alpha = 1;
    }
    
    public IEnumerator FadeOut(float time)
    {
        while(canvasGroup.alpha < 1) //canvas alphasını time süresi dolana kadar 1 e cıkar.
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float time)
    {
        while(canvasGroup.alpha  > 0) // canvas alphasını time süresi dolana kadar 0'a indir.
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
   
   
}
}
