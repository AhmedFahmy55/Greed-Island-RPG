using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using static RPG.Quests.QuestStatus;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour,ISaveable
    {
        List<QuestStatus> quests = new List<QuestStatus>();

        public event Action OnUpdateQuests;




        

        private bool HasQuest(Quest newQuest)
        {
            return GetQuestStatus(newQuest) != null; 
        }

        private QuestStatus GetQuestStatus(Quest newQuest)
        {
            foreach (var quest in quests)
            {
                if(quest.GetQuest() == newQuest) return quest ; 
            }
            return null;
        }

        public void AddQuest(Quest newQuest)
        {
           if(HasQuest(newQuest)) return;
            
            quests.Add(new QuestStatus(newQuest));
            OnUpdateQuests?.Invoke();
        }

        public IEnumerable<QuestStatus> GetQuests()
        {
            return quests;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus questStatus = GetQuestStatus(quest);
            if(questStatus != null)
            {
                questStatus.CompleteObjective(objective);
                OnUpdateQuests?.Invoke();
            }
        }

        public object CaptureState()
        {
            List<QuestStatusRecord> questList = new List<QuestStatusRecord>();
            foreach (var quest in quests)
            {
                questList.Add(quest.CaptureState());
            }
            return questList;
        }

        public void RestoreState(object state)
        {
            var questList =  state as List<QuestStatusRecord>;
            if(questList == null) return;
            quests.Clear();
            foreach(var quest in questList)
            {
                quests.Add(new QuestStatus(quest));
            }
            OnUpdateQuests?.Invoke();
        }
    }
}
