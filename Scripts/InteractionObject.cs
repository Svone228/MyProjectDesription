using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class InteractionObject : MonoBehaviour
{
    protected GameObject border;

    //Use with this script triger layerMask 8; 


    public virtual void MouseEnter()
    {
    }
    public virtual void MouseExit()
    {
        if (border != null)
            Destroy(border);

    }
    public virtual void Use()
    {
        Debug.Log("Use");
    }
    protected void CreateBorder()
    {
        border = Instantiate(gameObject, gameObject.transform);
        border.transform.localPosition = new Vector3(0, 0, 0);
        border.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        var euler = border.transform.localRotation;
        euler.eulerAngles = new Vector3(0, 0, 0);
        border.transform.localRotation = euler;
        border.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        border.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        var mass = border.GetComponents<Collider2D>();
        for (int i = 0; i < mass.Length; i++)
        {
            DestroyImmediate(mass[i].GetComponent<Collider2D>());
        }
        Destroy(border.GetComponent<ShadowCaster2D>());
    }
    protected void CreateBorder(Color color)
    {
        border = Instantiate(gameObject, gameObject.transform);
        border.transform.localPosition = new Vector3(0, 0, 0);
        border.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        var euler = border.transform.localRotation;
        euler.eulerAngles = new Vector3(0, 0, 0);
        border.transform.localRotation = euler;
        border.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        border.GetComponent<SpriteRenderer>().color = color;
        var mass = border.GetComponents<Collider2D>();
        for (int i = 0; i < mass.Length; i++)
        {
            DestroyImmediate(mass[i].GetComponent<Collider2D>());
        }
        Destroy(border.GetComponent<ShadowCaster2D>());
    }
    protected void CreateBorder(Color color,Vector3 scale)
    {
        border = Instantiate(gameObject, gameObject.transform);
        border.transform.localPosition = new Vector3(0, 0, 0);
        border.transform.localScale = scale;
        var euler = border.transform.localRotation;
        euler.eulerAngles = new Vector3(0, 0, 0);
        border.transform.localRotation = euler;
        border.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        border.GetComponent<SpriteRenderer>().color = color;
        var mass = border.GetComponents<Collider2D>();
        for (int i = 0; i < mass.Length; i++)
        {
            DestroyImmediate(mass[i].GetComponent<Collider2D>());
        }
        Destroy(border.GetComponent<ShadowCaster2D>());
    }
}
