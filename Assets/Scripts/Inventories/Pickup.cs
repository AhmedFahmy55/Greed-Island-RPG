using UnityEngine;
using RPG.Control;

namespace RPG.Inventories
{
    /// <summary>
    /// To be placed at the root of a Pickup prefab. Contains the data about the
    /// pickup such as the type of item and the number.
    /// </summary>
    public class Pickup : MonoBehaviour,IInteractable
    {
        InventoryItem item;
        int number = 1;

        Inventory inventory;


        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }


        
        public void Setup(InventoryItem item, int number)
        {
            this.item = item;
            if (!item.IsStackable())
            {
                number = 1;
            }
            this.number = number;
        }

        public InventoryItem GetItem()
        {
            return item;
        }

        public int GetNumber()
        {
            return number;
        }

        private bool PickupItem()
        {
            bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
            return foundSlot;
        }

        public bool CanBePickedUp()
        {
            return inventory.HasSpaceFor(item);
        }

        public bool Interact(PlayerController controller)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                PickupItem() ;
            }
            
            return true;
        }

        public CursorType GetCursorType()
        {
            if(CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            return CursorType.FullPickup;
        }
    }
}