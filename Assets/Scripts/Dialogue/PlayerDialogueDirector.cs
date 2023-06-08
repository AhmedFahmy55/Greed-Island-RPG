using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RPG.Dialogues
{
    public class PlayerDialogueDirector : MonoBehaviour
    {
        Dialogue currentDialogue;
        DialogueNode currentNode;
        NPCDialougeDirector npcDialougeDirector;
        public  event Action OnUpdateUI ;




        public void StartDialogue(NPCDialougeDirector npc,Dialogue newDialogue)
        {
            this.npcDialougeDirector = npc;
            currentDialogue = newDialogue;
            currentNode = newDialogue.GetRootNode();
            OnUpdateUI?.Invoke();
        }

        public void QuitDialogue()
        {
            npcDialougeDirector = null;
            currentDialogue = null ;
            currentNode = null ;
            OnUpdateUI?.Invoke();
        }
        public string GetText()
        {
            if(currentDialogue == null) return "" ;
            return currentNode.GetText();
        }

        public void SelectNode(DialogueNode selctedNode)
        {
            TriggerNodeAction(selctedNode.GetEvent());
            DialogueNode[] nodes = currentDialogue.GetNPCNodes(selctedNode).ToArray();
            if(nodes.Count() > 0)
            {
                
                int randomResponse = UnityEngine.Random.Range(0,nodes.Count());
                currentNode = nodes[randomResponse];
                OnUpdateUI?.Invoke();

            }
            
        }

        private void TriggerNodeAction(string action)
        {
            if(action == "") return;
            foreach (var trigger in npcDialougeDirector.GetComponents<DialougeTrigger>())
            {
                trigger.Trigger(action);

            }
        }

        public void GoNext()
        {
            SelectNode(currentNode);
        }
        public bool HasNext()
        {
            int i = GetPlayerRsponses().ToArray().Count();
            return i == 0 ;
        }

        public IEnumerable<DialogueNode> GetPlayerRsponses()
        {
             foreach (var node in currentDialogue.GetPlayerNodes(currentNode))
             {
                yield return node;
             }
        }

        public bool IsActive()
        {
            return currentDialogue != null ;
        }
    }   
}
