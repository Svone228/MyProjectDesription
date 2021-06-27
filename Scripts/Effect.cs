using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sqare_obj;
    GameObject Top_Square;
    GameObject Bottom_Square;
    public void OpenEyeEffects()
    {
        StartPosition();
    }
    void StartPosition() 
    {
        Top_Square = Instantiate(sqare_obj);
        Top_Square.transform.SetParent(gameObject.transform);
        Top_Square.GetComponent<RectTransform>().localPosition = new Vector3(0, 270, 0);
        Bottom_Square = Instantiate(sqare_obj);
        Bottom_Square.transform.SetParent(gameObject.transform);
        Bottom_Square.GetComponent<RectTransform>().localPosition = new Vector3(0, -270, 0);
        Bottom_Square.transform.localScale = new Vector3(1, 1, 1);
        Top_Square.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(EffectRoutine());
    }
    IEnumerator EffectRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        var hero = GameObject.FindGameObjectWithTag("Hero");
        hero.GetComponent<Controller>().SetMoveBlock();
        for (int i = 0; i < 50; i++)
        {
            Top_Square.transform.position = new Vector3(Top_Square.transform.position.x, Top_Square.transform.position.y + 6, Top_Square.transform.position.z);
            Bottom_Square.transform.position = new Vector3(Bottom_Square.transform.position.x, Bottom_Square.transform.position.y - 6, Bottom_Square.transform.position.z);
            yield return null;
        }
        Destroy(Top_Square);
        Destroy(Bottom_Square);
        hero.GetComponent<Controller>().RemoveMoveBlock();
        yield break;
    }
}
