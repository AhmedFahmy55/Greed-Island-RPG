using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI.Quests
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI questName;
        [SerializeField] TextMeshProUGUI progress;

        QuestStatus quest;
        public void SetUp(QuestStatus newQuest)
        {
            this.quest = newQuest;
            questName.text = newQuest.GetQuest().GetTitle();
            progress.text =newQuest.GetFinishedObjectivesNumb()+ "/" + newQuest.GetQuest().GetObjectivesCount();

        }

        public QuestStatus GetQuestStatus()
        {
            return quest;
        }
    }
}
