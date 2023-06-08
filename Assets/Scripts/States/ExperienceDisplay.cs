using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.States.UI
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience _experience ;
        TextMeshProUGUI _healthText;

        private void Awake() 
        {
            _experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            _healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() 
        {
           _healthText.text = _experience.GetExperience().ToString();    
        }
    }
}
