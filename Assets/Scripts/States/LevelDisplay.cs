using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.States
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStates _baseStates ;
        TextMeshProUGUI _healthText;

        private void Awake() 
        {
            _baseStates = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStates>();
            _healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() 
        {
           _healthText.text = _baseStates.GetLevel().ToString();    
        }
    }
}
