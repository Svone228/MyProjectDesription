using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Button_ShowObject : MonoBehaviour
{
    Button button;
    public GameObject Object;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskClick);
    }

    void TaskClick()
    {
        if (Object.activeSelf)
        {
            Object.SetActive(false);
        }
        else
        {
            Object.SetActive(true);
        }
    }
}
