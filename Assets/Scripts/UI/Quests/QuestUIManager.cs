using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;
using System;
using System.Linq;

namespace RPG.UI.Quests
{
    public class QuestUIManager : MonoBehaviour
    {
        [SerializeField] QuestUI questPrefap;
        QuestList questList;



        private void Awake() 
        {
            questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
            questList.OnUpdateQuests += UpdateQuests;
        }
        
        private void Start() 
        {
            UpdateQuests();
        }

        private void UpdateQuests() 
        {
            Debug.Log("enablig quest update");

            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }
            foreach (var quest in questList.GetQuests())
            {
                QuestUI instanceQuest = Instantiate(questPrefap,transform);
                instanceQuest.SetUp(quest);
            }
        }
    }
}
