using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPG.Combat
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health ;

        Canvas canvas;
        Slider healthSlider;

        private void Awake() 
        {
            healthSlider = GetComponentInChildren<Slider>();
            canvas = GetComponent<Canvas>();
        }
        
        private void Update() 
        {
            if( Mathf.Approximately(health.GetHealthFraction(),1) || Mathf.Approximately(health.GetHealthFraction(),0))
            {
                canvas.enabled = false;
                return;
            } 
            canvas.enabled = true ;
            healthSlider.value = health.GetHealthFraction();
        }

        
    }
}
