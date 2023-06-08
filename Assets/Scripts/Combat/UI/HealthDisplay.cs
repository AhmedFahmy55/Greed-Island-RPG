using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class HealthDisplay : MonoBehaviour
    {
        
        Health health ;
        TextMeshProUGUI healthText;

        private void Awake() 
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() 
        {
           healthText.text = health.GetCurrentHealth() + " / " + health.GetMaxHeath();    
        }
    }
}
