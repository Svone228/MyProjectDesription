using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInteraction : MonoBehaviour
{
    GameObject currentObj = null;
    int layerMask = 1 << 8; 

    // Update is called once per frame
    void Update()
    {
        SelectObject();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentObj != null) 
            {
                currentObj.GetComponent<InteractionObject>().Use();
            }
        }
    }
    void SelectObject() 
    {
        Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D Hit = Physics2D.Raycast(cubeRay, Vector2.zero, Mathf.Infinity, layerMask);
        if (Hit)
        {
            if (Hit.collider.gameObject == currentObj)
            {
            }
            else
            {
                if (currentObj != null)
                    currentObj.GetComponent<InteractionObject>().MouseExit();
                currentObj = Hit.collider.gameObject;
                currentObj.GetComponent<InteractionObject>().MouseEnter();
            }
        }
        else
        {
            if (currentObj != null)
            {
                currentObj.GetComponent<InteractionObject>().MouseExit();
                currentObj = null;
            }
        }
    }
}
