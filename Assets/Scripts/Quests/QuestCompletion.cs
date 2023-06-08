using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
       
       [SerializeField] Quest quest;
       [SerializeField] string objective;
        QuestList questList;



        private void Awake() 
        {
           questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
        }

       public void CompleteObjective()
       {
            questList.CompleteObjective(quest,objective);
       }
    }
}
