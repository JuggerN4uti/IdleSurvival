using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestones : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Milestones")]
    public bool[] milestoneComplete; // 0 - gold, 1 - slained
    public int[] milestoneProgress, milestoneGoal;

    public void ProgressMilestone(int ID, int amount)
    {
        milestoneProgress[ID] += amount;
        if (milestoneProgress[ID] >= milestoneGoal[ID])
            milestoneComplete[ID] = true;
    }

    public void CompleteMilestone(int ID)
    {

    }
}
