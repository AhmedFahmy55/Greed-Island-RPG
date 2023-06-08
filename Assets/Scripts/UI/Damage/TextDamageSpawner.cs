using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.UI.DamageText
{
    public class TextDamageSpawner : MonoBehaviour
    {
        [SerializeField] DamageText text = null;

        

        public void Spwan(float Damage)
        {
            DamageText insctance = Instantiate<DamageText>(text,transform);
            insctance.SetTextValue(Damage);
            
        }
    }
}
