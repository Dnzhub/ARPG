using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
   public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] GameObject TargetToDestroy = null;
    void Update()
    {
       if(!GetComponent<ParticleSystem>().IsAlive()) //Partikul efekti süresi bitince yok olacak.
       {
           if(TargetToDestroy != null)
           {
               Destroy(TargetToDestroy);
           }
           else
           {
               Destroy(gameObject);
           }
           
           
       } 
    }
} 
}

