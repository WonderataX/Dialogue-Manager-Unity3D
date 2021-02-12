using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool title;
    public bool subtitle;
    public AudioSource source;

    [HideInInspector]
    public bool dialogueEnd;

   public bool dialogueStarted=false;


    public bool end = false;

    private void Start()
    {
      
    }

    public  void TriggerDialogue()
    {
        dialogueStarted = true;
        FindObjectOfType<DialogueManager>().CanvasBools(title,subtitle);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue,source);
        
    }

    private void Update()
    {
        initBool();

    }

    void initBool()
    {
        dialogueEnd = FindObjectOfType<DialogueManager>().End();
        if (dialogueStarted && dialogueEnd)
        {
            end = true;
            dialogueStarted = false;
        }
    }


    
}
