using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
    
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour,IInteractable
    {

        Health health;


        private void Awake() 
        {
            health = GetComponent<Health>();
        }
        public bool Interact(PlayerController controller)
        {
            Fighter fighter = controller.GetComponent<Fighter>();

            if(!fighter.CanAttack(health)) return false;
          
            if(Input.GetMouseButtonDown(0))
            {
                    
                fighter.Attack(health);
                    
            }
            return true;
        }

        public CursorType GetCursorType()
        {
           return CursorType.Combat;
        }
    }
}
