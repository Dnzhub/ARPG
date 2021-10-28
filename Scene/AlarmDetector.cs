using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.warHorn();
            Destroy(gameObject);
        }
    }
}
