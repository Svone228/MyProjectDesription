using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : InteractionObject
{
    // Start is called before the first frame update
    protected GameObject canvas;
    public override void Use()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvas.GetComponent<StartTradingScript>().StartTrade();
    }
    private void Update()
    {
        
    }
    public override void MouseEnter()
    {
        base.CreateBorder(new Color(0,0,0),new Vector3(1.05f,1.03f,1));
    }
}
