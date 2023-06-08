using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Dialogues
{
    public class NPCDialougeDirector : MonoBehaviour, IInteractable
    {
        [SerializeField] Dialogue dialogue;

       
        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool Interact(PlayerController controller)
        {
            PlayerDialogueDirector playerDialogueDirector = controller.GetComponent<PlayerDialogueDirector>();
            
            if(dialogue == null) return false ;
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerDialogueDirector.StartDialogue(this,dialogue);
                
            }
            return true;
        }
    }
}
