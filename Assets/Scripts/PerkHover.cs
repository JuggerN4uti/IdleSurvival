using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerkHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillTree SkillTreeScript;
    public int perkId;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTreeScript.PerkHovered(perkId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTreeScript.Unhovered();
    }
}
