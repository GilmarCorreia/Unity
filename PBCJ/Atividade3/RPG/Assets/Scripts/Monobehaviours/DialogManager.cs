using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogText;

    [HideInInspector]
    public static Player player;

    public Animator animator;

    public static bool dialogueHasEnded = true;

    private AudioSource dialogueSound;
    public AudioClip beepSound;

    //FIFO
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        dialogueSound = allMyAudioSources[0];
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueHasEnded = false;
       
        animator.SetBool("IsOpen", true); 
        
        //player.GetComponent<PlayerMovement>().showDialogue();
        //player.GetComponent<Shooting>().showDialogue();

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (!dialogueSound.isPlaying)
            dialogueSound.PlayOneShot(beepSound);

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
 
    }

    IEnumerator TypeSentence (string sentence)
    {

        dialogText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {

        animator.SetBool("IsOpen", false);
        dialogueSound.Stop();

        //player.GetComponent<PlayerMovement>().stopDialogue();
        //player.GetComponent<Shooting>().stopDialogue();

        dialogueHasEnded = true;
    }
}
