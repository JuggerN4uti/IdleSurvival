using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Storage StorageScript;
    public int slotId;

    public void Clicked()
    {
        StorageScript.SlotClicked(slotId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StorageScript.SlotHovered(slotId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StorageScript.Unhovered();
    }
}
