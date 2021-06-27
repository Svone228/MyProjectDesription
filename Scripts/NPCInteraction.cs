using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : InteractionObject
{
    int DialogueNumber;
    public string[] Dialogues;
    Dialogue dialogue;
    void Start()
    {
        DialogueNumber = 0;
        dialogue = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {
        if (DialogueNumber != Dialogues.Length) 
        {
            dialogue.StartDialogue(Dialogues[DialogueNumber]);
            DialogueNumber++;
        }
    }
}
