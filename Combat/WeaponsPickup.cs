using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponsPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        Collider m_collider;

        private void Start()
        {
          m_collider = GetComponent<Collider>();  
        }
        
        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                other.GetComponent<Figther>().EquipWeapon(weapon); //Yerden silah alma
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }
        private IEnumerator HideForSeconds(float Seconds) // Silah yerden alınınca belli bi süre yok et ve tekrar spawn et.
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(Seconds);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool ShouldShow) 
        {
            m_collider.enabled = ShouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(ShouldShow);
            }
        }
    }

}
