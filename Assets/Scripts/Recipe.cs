using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    [Header("Crafted")]
    public bool eqItem;
    public int craftedID, craftedCount;

    [Header("Recipe")]
    public int uniqueMaterials;
    public bool[] eqMaterial;
    public int[] materialID, materialCount;
    public int craftDuration, craftedXP;
}
