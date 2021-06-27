using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionList : InteractionObject 
{

    /// <summary>
    /// DONT USE THIS 
    /// DONT USE THIS 
    /// </summary>



    public GameObject list;
    public Canvas canvas;
    public string Text;
    // Start is called before the first frame update
    void Start()
    {
        Text = "Test Text";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void MouseEnter()
    {
        GameObject.Instantiate(gameObject, gameObject.transform);
        border = GameObject.Instantiate(gameObject, gameObject.transform);
        border.transform.localPosition = new Vector3(0, 0, 0);
        border.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        border.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        border.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
        GameObject.DestroyImmediate(border.GetComponent<BoxCollider2D>());
    }
    public override void MouseExit()
    {
        GameObject.Destroy(border);
    }

    public override void Use()
    {
        Debug.Log("Use");
        list.SetActive(true);
        list.transform.SetParent(canvas.transform);
        list.transform.localPosition = new Vector3(0, 0, 0);
        list.GetComponentInChildren<Text>().text = Text;
    }
}
