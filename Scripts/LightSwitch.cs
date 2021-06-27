using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractionObject
{
    // Start is called before the first frame update
    public Sprite enabledSprite;
    public Sprite disabledSprite;
    public bool StartState;
    private bool state;


    void Start()
    {
        if (StartState) 
        {   
            GetComponent<SpriteRenderer>().sprite = enabledSprite;
            transform.GetChild(0).gameObject.SetActive(true);
            state = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
            transform.GetChild(0).gameObject.SetActive(false);
            state = false;
        }
    }

    // Update is called once per frame
    public override void Use()
    {
        SwitchState();
    }
    void SwitchState() 
    {
        if (state) 
        {
            GetComponent<SpriteRenderer>().sprite = disabledSprite;
            transform.GetChild(0).gameObject.SetActive(false);
            state = false;
        }
        else 
        {
            GetComponent<SpriteRenderer>().sprite = enabledSprite;
            transform.GetChild(0).gameObject.SetActive(true);
            state = true;
        }
    }
}
