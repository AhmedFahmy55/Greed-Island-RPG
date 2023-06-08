using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Combat
{
    public class EnemyHeathDisplay : MonoBehaviour
    {
        
        [SerializeField] TextMeshProUGUI healthText;

        Fighter target ;

        private void Awake() 
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }
        
        
        
        private void Update() 
        {
           if(target.GetTargetHealth() == null)
           {
                healthText.text = "N / A" ;
                return ;
           }
           
           healthText.text = target.GetTargetHealth().GetCurrentHealth().ToString();
                
        }
    }
}


