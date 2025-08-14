using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestones : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Milestones")]
    public bool[] milestoneComplete; // 0 - gold, 1 - slained
    public int[] milestoneProgress, milestoneGoal, milestonesReached;

    [Header("UI")]
    public Image[] MilestoneBarFill;
    public Button[] MilestoneBarButton;
    public TMPro.TextMeshProUGUI[] MilestoneProgressText, BonusFromMilestone;

    [Header("Milestones Goals")]
    public int[] goldToCollect;
    public int[] enemiesToSlain;

    public void ProgressMilestone(int ID, int amount)
    {
        milestoneProgress[ID] += amount;
        if (milestoneProgress[ID] >= milestoneGoal[ID])
            milestoneComplete[ID] = true;

        if (PlayerScript.windowOpened[6])
            DisplayMilestone(ID);
    }

    public void CompleteMilestone(int ID)
    {
        milestonesReached[ID]++;
        BonusFromMilestone[ID].text = "+" + (milestonesReached[ID] * 5).ToString("0") + "%";

        switch (ID)
        {
            case 0:
                PlayerScript.goldIncrease += 0.05f;
                milestoneGoal[0] = goldToCollect[milestonesReached[0]];
                break;
            case 1:
                PlayerScript.damageIncrease += 0.05f;
                milestoneGoal[1] = enemiesToSlain[milestonesReached[1]];
                break;
        }

        milestoneComplete[ID] = false;
        MilestoneBarButton[ID].interactable = false;
        ProgressMilestone(ID, 0);
    }

    public void DisplayWindow()
    {
        for (int i = 0; i < milestoneComplete.Length; i++)
        {
            DisplayMilestone(i);
        }
    }

    void DisplayMilestone(int ID)
    {
        MilestoneBarFill[ID].fillAmount = (milestoneProgress[ID] * 1f) / (milestoneGoal[ID] * 1f);
        MilestoneProgressText[ID].text = milestoneProgress[ID].ToString("0") + "/" + milestoneGoal[ID].ToString("0");
        if (milestoneComplete[ID])
            MilestoneBarButton[ID].interactable = true;
    }
}
