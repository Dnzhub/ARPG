using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class PortalScript : MonoBehaviour
{   
    enum DestinationIdentifier
    {
        A,B,C,D,E
    }
    [SerializeField] int LoadToNextScene = -1; 
    [SerializeField] Transform SpawnPoint;
    [SerializeField] DestinationIdentifier destination;

    [SerializeField] float FadeOutTime = 1f;
    [SerializeField] float FadeInTime = 2f;
    [SerializeField] float FadeWaitTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
       {
          StartCoroutine(Transition());
       }
    }
    private IEnumerator Transition()
    {
        if(LoadToNextScene < 0)
        {
            Debug.LogError("Scene can not set");
            yield break;
        }
        
        DontDestroyOnLoad(gameObject);
        Fader fader = FindObjectOfType<Fader>();
        

        yield return fader.FadeOut(FadeOutTime); // Fade out gercekleşene kadar bekle
        
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save(); // Sahne değişmeden önceki durumu kaydet

        yield return SceneManager.LoadSceneAsync(LoadToNextScene); // Sahne değişimi olana kadar portal objesini yok etme.
        yield return new WaitForEndOfFrame();
        wrapper.Load(); // Sahne değiştikten önceki durumu yükle

        PortalScript otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        wrapper.Save(); //Tekrar save ettikki yeni sahneye gecinceki son durumu kaydet
        
        yield return new WaitForSeconds(FadeWaitTime);
        yield return fader.FadeIn(FadeInTime); // Fade in gercekleşene kadar bekle

        Destroy(gameObject);
    }

        private void UpdatePlayer(PortalScript otherPortal)
        {
           GameObject player = GameObject.FindWithTag("Player");
           player.GetComponent<NavMeshAgent>().enabled = false;
           player.GetComponent<NavMeshAgent>().Warp(otherPortal.SpawnPoint.position); //Playerı(NavmeshAgent kullandık bug engellemek icin) diğer otherportalın pozisyon ve rotasyonuna tası.                                                                             //Warp ile navmeshagent konumunu belirledik
           player.transform.rotation = otherPortal.SpawnPoint.rotation;
           player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private PortalScript GetOtherPortal()
        {
           foreach(PortalScript portal in FindObjectsOfType<PortalScript>())
           {
               if(portal == this) continue; // portal mevcut portal ise for looptan cık
               if(portal.destination != destination) continue; //Sadece birbirlerinin destination harfi uyuşan portallar birbiri ile bağlantı kurabilir.A destination yalnızca A destinationa gidebilir gibi.
               return portal; // mevcut portal değilde bir sonraki portal ise calıstır.
           }
           return null; // Eger haritada portal yok ise null calıstır.
        }
    }

}

