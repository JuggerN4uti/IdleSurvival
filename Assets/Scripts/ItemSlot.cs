using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Storage StorageScript;
    public int slotId;
    public bool eq, worn;

    public void Clicked()
    {
        if (eq)
            StorageScript.EqSlotClicked(slotId);
        else StorageScript.SlotClicked(slotId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StorageScript.SlotHovered(slotId, eq, worn);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StorageScript.Unhovered();
    }
}
