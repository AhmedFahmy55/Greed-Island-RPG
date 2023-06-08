using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode key ;
        [SerializeField] GameObject target;



        private void Update() 
        {
            if(Input.GetKeyDown(key))
            {
                ShowHiideObj();
            }
        }

        public void ShowHiideObj()  
        {
            target.SetActive(!target.activeSelf);
        }
    }
}
