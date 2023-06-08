using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Tooltips;
using RPG.Quests;

namespace RPG.UI.Quests
{
    public class QuestTooltip : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus quest = GetComponent<QuestUI>().GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(quest);
        }
    }
}
