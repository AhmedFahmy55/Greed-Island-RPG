using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        QuestList questList;

        private void Awake() 
        {
           questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
        }

        public void GiveQuest()
        {
            questList.AddQuest(quest);
        }
    }
}
