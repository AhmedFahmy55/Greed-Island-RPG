using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat{

    public class WeaponPickup : MonoBehaviour,IInteractable
    {
        [SerializeField] private WeaponSo weaponPickp = null ;
        [SerializeField] private float spwanTime;

        private void OnTriggerEnter(Collider other) 
        {
            if(other.gameObject.tag == "Player")
            {
                EquipWeapon(other.GetComponent<Fighter>());
            }
        }

        private void EquipWeapon(Fighter other)
        {
            other.EquiepWeapon(weaponPickp);
            StartCoroutine(HideForSeconds(spwanTime));
        }

        IEnumerator HideForSeconds(float time) 
        {
            ShowObj(false);
            yield return new WaitForSeconds(time);
            ShowObj(true);
        }

        private void ShowObj(bool shouldShow)
        {
            GetComponent<Collider>().enabled=shouldShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool Interact(PlayerController controller)
        {
            if(Input.GetMouseButtonDown(0))
            {
                EquipWeapon(controller.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
           return CursorType.Pickup;
        }
    }


}