using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Storage StorageScript;
    public Smelting SmeltingScript;
    public int slotId;
    public bool eq, worn, furnace;

    public void Clicked()
    {
        if (furnace)
        {
            SmeltingScript.SlotClicked(slotId);
        }
        else
        {
            if (eq)
                StorageScript.EqSlotClicked(slotId);
            else StorageScript.SlotClicked(slotId);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!furnace)
            StorageScript.SlotHovered(slotId, eq, worn);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!furnace)
            StorageScript.Unhovered();
    }
}
