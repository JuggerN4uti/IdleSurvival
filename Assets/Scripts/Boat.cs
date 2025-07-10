using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    public Player PlayerScript;
    public bool built;

    public SpriteRenderer BoatImage;
    public GameObject MaterialsObject;

    public void BoatClicked()
    {
        if (!built)
        {
            if (PlayerScript.StorageScript.itemsCount[0] >= 40 && PlayerScript.StorageScript.itemsCount[3] >= 2)
                Repair();
        }
        else PlayerScript.WorldView();
    }

    void Repair()
    {
        PlayerScript.StorageScript.UseItem(0, 40);
        PlayerScript.StorageScript.UseItem(3, 2);
        built = true;
        BoatImage.color = new Color(1f, 1f, 1f, 1f);
        MaterialsObject.SetActive(false);
    }
}
