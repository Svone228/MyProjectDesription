using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : InteractionObject
{
    Vector3 starposition;
    // Start is called before the first frame update
    Rigidbody2D rb;
    public bool Stat;
    void Awake()
    {
        Stat = !Stat;
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = starposition;
        rb.angularDrag = 5;
        
    }

    // Update is called once per frame
    public override void Use()
    {
        if (Stat) 
        {
            transform.rotation = transform.parent.rotation;
            
            rb.angularVelocity = 0;
        }
    }
    private void Update()
    {
        if (Stat)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
