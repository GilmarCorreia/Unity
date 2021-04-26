using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    [HideInInspector]
    public static DialogManager dialogManager;

    void Start(){
        dialogManager = GameObject.Find("DialogManager").GetComponent<DialogManager>();
    }
    public void TriggerDialogue()
    {
        dialogManager.StartDialogue(dialogue);        
    }
}
