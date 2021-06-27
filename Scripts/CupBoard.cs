using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBoard : InteractionObject
{
    bool state=false;
    public override void Use()
    {
        if (!state)
        {
            GameObject innerObject = gameObject.transform.GetChild(0).gameObject;
            var temp = innerObject.transform.localPosition;
            temp.y = -0.7f;
            innerObject.transform.localPosition = temp;
            state = true;
        }
        else
        {
            GameObject innerObject = gameObject.transform.GetChild(0).gameObject;
            var temp = innerObject.transform.localPosition;
            temp.y = -0.2f;
            innerObject.transform.localPosition = temp;
            state = false;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
