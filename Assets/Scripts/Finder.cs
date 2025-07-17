using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finder : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;
    public Mobile MobileFound;
    public Resource ResourceFound;

    [Header("Stats")]
    public int taskFinderID;
    public Vector3 scaleChange;

    void Update()
    {
        transform.localScale += scaleChange * Time.deltaTime;
        if (transform.localScale.x >= 20f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Mobile" && taskFinderID == 0)
            FoundMob(other);
        if (other.transform.tag == "Resource" && taskFinderID == 1)
            FoundResource(other);
    }

    void FoundMob(Collider2D other)
    {
        MobileFound = other.GetComponent(typeof(Mobile)) as Mobile;
        PlayerScript.ChangeTask();
        MobileFound.Shadow.color = new Color(1f, 0f, 0f, 0.49f);
        PlayerScript.MobileTargeted = MobileFound;
        PlayerScript.fighting = true;
        Destroy(gameObject);
    }

    void FoundResource(Collider2D other)
    {
        ResourceFound = other.GetComponent(typeof(Resource)) as Resource;
        PlayerScript.ChangeTask();
        ResourceFound.Shadow.color = new Color(0f, 1f, 0f, 0.49f);
        PlayerScript.ResourceTargeted = ResourceFound;
        PlayerScript.SelectNewTask(ResourceFound.resourceType);
        PlayerScript.collecting = true;
        Destroy(gameObject);
    }
}
