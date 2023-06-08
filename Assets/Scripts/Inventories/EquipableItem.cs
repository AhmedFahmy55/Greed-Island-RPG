using UnityEngine;
using RPG.States;
using System.Collections.Generic;

namespace RPG.Inventories
{
    /// <summary>
    /// An inventory item that can be equipped to the player. Weapons could be a
    /// subclass of this.
    /// </summary>
    [CreateAssetMenu(menuName = ("RPG/InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem ,IModifireProvider
    {
        // CONFIG DATA
        [Tooltip("Where are we allowed to put this item.")]
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;
        [SerializeField] EquipStates [] addtiveStates;
        [SerializeField] EquipStates [] percentageStates;



        [System.Serializable]
        struct EquipStates
        {
            public State state;
            public float value;
        }

       

        // PUBLIC

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }

         public IEnumerable<float> GetAddtiveModifires(State state)
        {
            foreach(var equipState in addtiveStates )
            {
                if(equipState.state != state) continue;
                yield return equipState.value;
            }
        }

        public IEnumerable<float> GetPercentagesModifires(State state)
        {
             foreach(var equipState in percentageStates )
            {
                if(equipState.state != state) continue;
                yield return equipState.value;
            }
        }
    }
}