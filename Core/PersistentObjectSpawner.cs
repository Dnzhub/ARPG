using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bir saneden baska bir sahneye gecerken Fader icin once bulundugumuz sahnede Fadeout Yapıp
//Diğer sahnede aynı obje ile Fadein Yapmamız gerekir.
//Bunun icin persistent(Kalıcı obje) olustururuz ki prefab yok olmasın

namespace RPG.Core
{
public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjectPrefab;
                        
    static bool hasSpawned = false;  //Static Unity sürekli bu veriyi hatırlayacaktır.private yada public yazsaydık class işi bitince bool da biticekti bu yüzden static kullandık.
    
    private void Awake()
    {
        if(hasSpawned) return; //Eger obje spawn olduysa scripti durdur.

            SpawnPersistentObjects();
            hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
}

