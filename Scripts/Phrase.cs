using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase : MonoBehaviour
{
    // Start is called before the first frame update
    public string phrase;
    public StartQuest mainscript;
    public GameObject nextTrigger;
    GameObject Canvas;
    Dialogue dialogue;
    private void Awake()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        dialogue = Canvas.GetComponent<Dialogue>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hero= GameObject.FindGameObjectWithTag("Hero");
        if (collision.gameObject == hero)
        {
            dialogue.StartDialogue(phrase);
            if (nextTrigger != null)
            {
                nextTrigger.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
