using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugQuestLog : MonoBehaviour
{
    [TextArea(5, 20)]
    public string questName;

    [TextArea(5, 20)]
    public string questDescription;

    [TextArea(5, 20)]
    public string questRecipe;

    void Start()
    {
        StartCoroutine(AddQuest(1));
    }

    private Quest getNext(int i) {
        Quest q = new Quest();
        q.questName = questName;
        q.questDescription = questDescription;
        q.howToUseText = questRecipe;
        q.questCategory = 0;
        q.objective = new Quest.Objective();
        q.objective.type = (Quest.Objective.Type)Random.Range(0,2);
        q.objective.amount = Random.Range(2,10);
        return q;
    }

    private IEnumerator AddQuest(int iter) {
        for (int i = 0; i < iter; i++) {
            QuestLog.AddQuest(getNext(i));
            yield return new WaitForSeconds(3f);
        }
    }
}
