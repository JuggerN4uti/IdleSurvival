using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Scripts")]
    public Storage StorageScript;

    [Header("Stats")]
    public int differentMaterials;
    public int[] materialID, materialCount;
    bool enough;

    [Header("Objects")]
    public GameObject DeactivateObject;
    public GameObject ActivateObject;

    public void Clicked()
    {
        enough = true;
        for (int i = 0; i < differentMaterials; i++)
        {
            if (StorageScript.itemsCount[materialID[i]] < materialCount[i])
                enough = false;
        }
        if (enough)
            Build();
    }

    void Build()
    {
        for (int i = 0; i < differentMaterials; i++)
        {
            StorageScript.UseItem(materialID[i], materialCount[i]);
        }
        DeactivateObject.SetActive(false);
        ActivateObject.SetActive(true);
    }
}
