using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    [HideInInspector]
    public static DialogManager dialogManager;

    bool hasStarted = false;

    void Update()
    {

        if (hasStarted && !DialogManager.dialogueHasEnded)
        {    
            Destroy(gameObject);
        }
    }
    public void TriggerDialogue()
    {
        hasStarted = true;
        dialogManager.StartDialogue(dialogue);        
    }
}
